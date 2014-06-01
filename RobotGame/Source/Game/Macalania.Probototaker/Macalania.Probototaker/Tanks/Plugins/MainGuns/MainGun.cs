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
        public Vector2 ProjectileStartPosition { get; set; }
        public override void Update(double dt)
        {
            base.Update(dt);

            Sprite.Rotation = Tank.TurretRotation + Tank.BodyRotation;
        }

        public virtual void Fire(Turret turret)
        {
            // Calculates the start position of the projectile
            float height = (turret.Sprite.Texture.Height / 2) + Sprite.Texture.Height + ProjectileStartPosition.Y;
            float width = (turret.ExtraPixelsTop / 2) + PluginPosition * Globals.PluginPixelWidth + Globals.PluginPixelWidth * Size / 2;
            width = width - turret.Sprite.Texture.Width / 2;
            width = -width;

            Vector2 projectileSpawnPosition = YunaMath.RotateVector2(new Vector2(width, height), Tank.GetTurrentBodyRotation());

            SpawnProjectile(projectileSpawnPosition + Tank.Position, -Tank.GetTurretDirection());
        }

        public virtual void SpawnProjectile(Vector2 position, Vector2 direction)
        {
        }
    }
}
