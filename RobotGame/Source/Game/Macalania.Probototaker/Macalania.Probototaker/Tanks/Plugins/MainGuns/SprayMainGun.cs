using Macalania.Probototaker.Projectiles;
using Macalania.YunaEngine;
using Macalania.YunaEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Plugins.MainGuns
{
    class SprayMainGun: MainGun
    {
        float TimeSinceLastBustShoot = 0;
        float BurstTimeInterval = 30;

        public SprayMainGun()
        {
            Size = 3;
            ProjectileStartPosition = new Vector2(0, 0);
            RateOfFire = 500;
            CoolDownRate = 1f;
        }
        public override void Load(ContentManager content)
        {
            Sprite = new Sprite(content.Load<Texture2D>("Textures/Tanks/MainGuns/SprayMainGun"));
            base.Load(content);
        }

        public override void Update(double dt)
        {
            base.Update(dt);
        }

        public override void Fire(Vector2 position, Vector2 direction)
        {
            base.Fire(position, direction);

            if (TimeSinceLastFire > RateOfFire)
            {
                ShellStarter ss = new ShellStarter(Tank, position, direction, 0.5f);

                YunaGameEngine.Instance.GetActiveRoom().AddGameObjectWhileRunning(ss);
                
                ShotFired();
            }
        }
    }
}
