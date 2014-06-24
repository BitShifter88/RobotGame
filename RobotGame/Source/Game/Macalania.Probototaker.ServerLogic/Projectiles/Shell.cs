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
    public class Shell : Projectile
    {
        public Shell(Room room, Tank tankSource, Vector2 position, Vector2 direction, float speed, ProjectileType type)
            : base(room, tankSource, position, direction, speed, type)
        {
            Flying = true;
        }

        protected override void OnCollisionWithShield(Shield s)
        {
            DestroyGameObject();
            base.OnCollisionWithShield(s);
        }

        public override void OnCollisionWithTank(Tank tank, TankComponent component)
        {
            if (component.CompType == TankComponentType.Amor && component.IsDestroyed == false)
            {
                DestroyGameObject();
            }
            else
            {
                base.OnCollisionWithTank(tank, component);
            }
        }
    }
}
