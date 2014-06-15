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
    public class ArtileryStarter : RocketBattery
    {
        public ArtileryStarter()
        {
            Size = 3;
            _rockets = new ArtileryProjectile[6];
            OriginOfset = new Vector2(0, 30);
            MaxCooldown = 5000;
            ComponentMaxHp = 100;
            _fireInterval = 400;
        }
        public override void Load(ResourceManager content)
        {
             Sprite = new Sprite(content.LoadYunaTexture("Textures/Tanks/Misc/artilery"));
             Sprite.DepthLayer = 0.3f;
            base.Load(content);
        }
        protected override void OnFireRockets()
        {
            Tank.ArtilleryFirering = true;
            base.OnFireRockets();
        }
        protected override void OnFieringEnd()
        {
            Tank.ArtilleryFirering = false;
            base.OnFieringEnd();
        }
        public override void Update(double dt)
        {
            base.Update(dt);
        }
        protected override void FireRocket(int index)
        {
            _rockets[index].Ignite(Tank.Position, Vector2.Distance(Tank.Position, _targetPosition) + GameRandom.GetRandomInt(-200, 200));
            _rockets[index].Sprite.SetOriginCenter();
            _rockets[index].Sprite.Rotation = Tank.GetTurrentBodyRotation() + MathHelper.ToRadians(180);
            Vector2 p = new Vector2(-((Sprite.Texture.Width / 2) - 2 - 3 - 10 * index), -Sprite.Origin.Y + _rockets[index].Sprite.Texture.Height/2);
            p = YunaMath.RotateVector2(p, Tank.GetTurrentBodyRotation() + MathHelper.ToRadians(180));
            _rockets[index].SetPosition(p + Tank.Position);

            _rockets[index].Direction = -Tank.GetTurretDirection();
            _rockets[index] = null;
        }
        public override bool Activate(Vector2 point, Tank target)
        {
            if (Tank.IsStandingStill() == false)
                return false;
            return base.Activate(point, target);
        }
        protected override void ReloadRockets()
        {
            for (int i = 0; i < _rockets.Length; i++)
            {
                ArtileryProjectile rocket2 = new ArtileryProjectile(RoomManager.Instance.GetActiveRoom(), Tank, new Vector2(0, 0), Tank.GetTurretDirection(), 0.0f);
                RoomManager.Instance.GetActiveRoom().AddGameObjectWhileRunning(rocket2);

                Vector2 org2 = new Vector2((Sprite.Texture.Width / 2) - 2 - 10 * i, Sprite.Origin.Y);
                rocket2.Sprite.Origin = org2;

                _rockets[i] = rocket2;
            }
        }
    }
}
