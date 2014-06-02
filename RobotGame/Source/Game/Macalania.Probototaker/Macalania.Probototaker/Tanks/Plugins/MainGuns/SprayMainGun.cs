using Macalania.Probototaker.Network;
using Macalania.Probototaker.Projectiles;
using Macalania.Probototaker.Tanks.Turrets;
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
        float BurstTimeInterval = 20;
        int burstShots = 0;
        Turret _turret;

        public SprayMainGun(Turret turret)
        {
            _turret = turret;
            Size = 3;
            ProjectileStartPosition = new Vector2(0, 0);
            RateOfFire = 500;
            CoolDownRate = 1f;
        }
        public override void Load(ContentManager content)
        {
            Sprite = new Sprite(content.Load<Texture2D>("Textures/Tanks/MainGuns/SprayMainGun"));
            Sprite.DepthLayer = 0.24f;
            base.Load(content);
        }

        public override void Update(double dt)
        {
            base.Update(dt);

            if (burstShots > 0 && TimeSinceLastBustShoot >= BurstTimeInterval)
            {
                float height = (_turret.Sprite.Texture.Height / 2) + Sprite.Texture.Height + ProjectileStartPosition.Y;
                float width = (_turret.ExtraPixelsTop / 2) + PluginPosition * Globals.PluginPixelWidth + Globals.PluginPixelWidth * Size / 2;
                width = width - _turret.Sprite.Texture.Width / 2;
                width = -width;

                Vector2 projectileSpawnPosition = YunaMath.RotateVector2(new Vector2(width, height), Tank.GetTurrentBodyRotation()) + Tank.Position;

                ShellStarter ss = new ShellStarter(Tank, projectileSpawnPosition, -YunaMath.RotateVector2(Tank.GetTurretDirection(), GameRandom.GetRandomFloat(0.2f) - 0.1f), 0.5f);
                YunaGameEngine.Instance.GetActiveRoom().AddGameObjectWhileRunning(ss);
                ShotFired();

                burstShots--;
                TimeSinceLastBustShoot = 0;
            }

            TimeSinceLastBustShoot += (float)dt;
        }

        public override void Fire(Vector2 position, Vector2 direction)
        {
            base.Fire(position, direction);

            if (TimeSinceLastFire > RateOfFire)
            {
                burstShots = 5;
            }
        }
    }
}
