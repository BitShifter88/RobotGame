using Macalania.Probototaker.Tanks.Turrets;
using Macalania.YunaEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Plugins.MainGuns
{
    class MainGun : Plugin
    {
        public float RateOfFire { get; set; }
        public float TimeSinceLastFire { get; set; }
        public Vector2 ProjectileStartPosition { get; set; }

        public float GunHeat { get; set; }
        public float CoolDownRate { get; set; }
        public bool Overheated { get; set; }

        public override void Update(double dt)
        {
            base.Update(dt);

            Sprite.Rotation = Tank.TurretRotation + Tank.BodyRotation;
            TimeSinceLastFire += (float)dt;

            if (Overheated == false)
            {
                GunHeat -= (float)dt * CoolDownRate;
            }
            else
            {
                GunHeat -= (float)dt * CoolDownRate / 2;
            }
            if (GunHeat < 0)
                GunHeat = 0;

            if (GunHeat <= 0)
                Overheated = false;
        }

        public virtual void FireRequest(Turret turret)
        {
            if (Overheated == false)
            {
                // Calculates the start position of the projectile
                float height = (turret.Sprite.Texture.Height / 2) + Sprite.Texture.Height + ProjectileStartPosition.Y;
                float width = (turret.ExtraPixelsTop / 2) + PluginPosition * Globals.PluginPixelWidth + Globals.PluginPixelWidth * Size / 2;
                width = width - turret.Sprite.Texture.Width / 2;
                width = -width;

                Vector2 projectileSpawnPosition = YunaMath.RotateVector2(new Vector2(width, height), Tank.GetTurrentBodyRotation()) + Tank.Position;

                Fire(projectileSpawnPosition, -Tank.GetTurretDirection());
            }
        }

        public virtual void Fire(Vector2 position, Vector2 direction)
        {
        }

        protected void ShotFired()
        {
            TimeSinceLastFire = 0;
            GunHeat += 10;
            if (GunHeat >= 100)
                Overheated = true;
        }
    }
}
