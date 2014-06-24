using Macalania.Probototaker.Effects;
using Macalania.Probototaker.Tanks;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Projectiles
{
    public class Rocket : Projectile
    {
        public float Imprecition { get; set; }
        public Vector2 OriginPosition { get; set; }
        public float FlyDistance { get; set; }

        public Rocket(Room room, Tank tankSource, Vector2 position, Vector2 direction, float speed, ProjectileType type)
            : base(room, tankSource, position, direction, speed, type)
        {
        }

        public override void Update(double dt)
        {
            if (Vector2.Distance(OriginPosition, Position) > FlyDistance)
            {
                Explode();
            }
            base.Update(dt);
        }

        public override void OnCollisionWithTank(Tank tank, TankComponent component)
        {
            base.OnCollisionWithTank(tank, component);
            Explode();
        }

        public virtual void Explode()
        {
            DestroyGameObject();
        }

        public void Ignite(Vector2 originPosition, float flyDistance)
        {
            OriginPosition = originPosition;
            FlyDistance = flyDistance;
            Flying = true;
        }

        protected override void OnCollisionWithShield(Shield s)
        {
            Explode();
            base.OnCollisionWithShield(s);
        }
    }
}
