using Macalania.Probototaker.Tanks.Plugins;
using Macalania.YunaEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.NewTurret
{
    public class MainGunNew : TurretModule
    {
        public float RateOfFire { get; set; }
        public float TimeSinceLastFire { get; set; }
        public Vector2 ProjectileStartPosition { get; set; }

        public float GunHeat { get; set; }
        public float CoolDownRate { get; set; }
        public bool Overheated { get; set; }

        public MainGunNew(PluginType type) : base (type)
        {
        
        }

        public override void Update(double dt)
        {
            Sprite.Rotation = Tank.TurretRotation + Tank.BodyRotation;
            Sprite.Position = Tank.Position;
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

            base.Update(dt);
        }

        public virtual void FireRequest(Turret turret)
        {
            if (Overheated == false)
            {
                // Calculates the start position of the projectile
                float height = GetCentredY() * 16;
                float width = (GetCentredX()+1) * 16;
                height = -height;
                width = -width;

                Vector2 dir = YunaMath.RotateVector2(new Vector2(width, height), Tank.GetTurrentBodyRotation());

                Vector2 projectileSpawnPosition = dir + Tank.Position;

                //dir.Normalize();
                //projectileSpawnPosition += dir * 5;

                Fire(projectileSpawnPosition, -Tank.GetTurretBodyDirection());
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
