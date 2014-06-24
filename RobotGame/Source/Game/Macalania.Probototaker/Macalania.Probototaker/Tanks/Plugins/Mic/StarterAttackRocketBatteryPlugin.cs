using Macalania.Probototaker.Network;
using Macalania.Probototaker.Projectiles;
using Macalania.YunaEngine;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Resources;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Plugins.Mic
{
    public class StarterAttackRocketBatteryPlugin : RocketBattery
    {
        public StarterAttackRocketBatteryPlugin()
            : base(PluginType.StarterAttackRocket)
        {
            Size = 2;
            _rockets = new AttackRocketProjectile[5];
            OriginOfset = new Vector2(0, 30);
            MaxCooldown = 5000;
            ComponentMaxHp = 100;
            _fireInterval = 100;
        }
        public override void Load(ResourceManager content)
        {
            Sprite = new Sprite(content.LoadYunaTexture("Textures/Tanks/Misc/starterAttackRocket"));
             Sprite.DepthLayer = 0.3f;
            base.Load(content);
        }

        protected override void FireRocket(int index)
        {
            _rockets[index].Ignite(Tank.Position, 1500);
            _rockets[index].Sprite.SetOriginCenter();
            _rockets[index].Sprite.Rotation = Tank.GetTurrentBodyRotation() + MathHelper.ToRadians(180);
            Vector2 p = new Vector2((Tank.Turret.Sprite.Texture.Width / 2) + 2 + 1.5f + 5 * index, -Sprite.Origin.Y + _rockets[index].Sprite.Texture.Height / 2);
            p = YunaMath.RotateVector2(p, Tank.GetTurrentBodyRotation() + MathHelper.ToRadians(180));
            _rockets[index].SetPosition(p + Tank.Position);

            _rockets[index].Direction = -Tank.GetTurretBodyDirection();
            _rockets[index] = null;
        }

        protected override void ReloadRockets()
        {
            for (int i = 0; i < _rockets.Length; i++)
            {
                AttackRocketProjectile rocket2 = new AttackRocketProjectile(RoomManager.Instance.GetActiveRoom(), Tank, new Vector2(0, 0), Tank.GetTurretBodyDirection(), 0.0f);
                RoomManager.Instance.GetActiveRoom().AddGameObjectWhileRunning(rocket2);

                Vector2 org2 = new Vector2(-(Tank.Turret.Sprite.Texture.Width / 2) - 2 - 5 * i, Sprite.Origin.Y);
                rocket2.Sprite.Origin = org2;

                _rockets[i] = rocket2;
            }
        }
    }
}
