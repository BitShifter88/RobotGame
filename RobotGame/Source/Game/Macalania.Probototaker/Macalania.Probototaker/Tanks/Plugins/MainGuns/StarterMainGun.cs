﻿using Macalania.Probototaker.Projectiles;
using Macalania.YunaEngine;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Plugins.MainGuns
{
    public class StarterMainGun : MainGun
    {
        public StarterMainGun()
        {
            Size = 2;
            ProjectileStartPosition = new Vector2(0, 0);
            RateOfFire = 500;
            CoolDownRate = 1;
        }
        public override void Load(ResourceManager content)
        {
            Sprite = new Sprite(content.LoadYunaTexture("Textures/Tanks/MainGuns/mainGunStarter"));
            Sprite.DepthLayer = 0.24f;
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
                ShellStarter ss = new ShellStarter(YunaGameEngine.Instance.GetActiveRoom(), Tank, position, direction, 1f);

                YunaGameEngine.Instance.GetActiveRoom().AddGameObjectWhileRunning(ss);
                TimeSinceLastFire = 0;
                ShotFired();
            }
        }
    }
}
