using Macalania.Probototaker.Projectiles;
using Macalania.Probototaker.Tanks.Turrets;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Resources;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Plugins.MainGuns
{
    public class MiniCanon : MainGunNew
    {
        Animation _ani;

        public MiniCanon(): base(Plugins.PluginType.MiniMainGun)
        {
            RequiredBricks = new List<Point>();
            RequiredBricks.Add(new Point(0, 3));
            RequiredBricks.Add(new Point(1, 3));

            Size = 1;
            ProjectileStartPosition = new Vector2(0, 0);
            RateOfFire = 300;
            CoolDownRate = 0.03f;
        }

        public override void AddComponents(Turret turret)
        {
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    turret.AddTurretComponent(new TurretComponent(), _x + i, _y + j);
                }
            }
        }

        public override void Load(ResourceManager content)
        {
            _ani = new Animation(32,9, 60, content.LoadYunaTexture("Textures/Tanks/MainGuns/MiniMainGun"));
            Sprite = _ani;
            Sprite.DepthLayer = 0.23f;

            base.Load(content);
        }

        public override void Fire(Vector2 position, Vector2 direction)
        {
            base.Fire(position, direction);

            if (TimeSinceLastFire > RateOfFire)
            {
                ShellStarter ss = new ShellStarter(RoomManager.Instance.GetActiveRoom(), Tank, position, direction);

                RoomManager.Instance.GetActiveRoom().AddGameObjectWhileRunning(ss);
                _ani.PlayAnimation();
                ShotFired();
            }
        }
    }
}
