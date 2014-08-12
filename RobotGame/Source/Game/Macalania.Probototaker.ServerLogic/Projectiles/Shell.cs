using Macalania.Probototaker.Effects;
using Macalania.Probototaker.Rooms;
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
            ProjectileFired();
        }

        protected override void OnCollisionWithShield(Shield s)
        {
            DestroyGameObject();
            base.OnCollisionWithShield(s);
        }

        public override void Load(YunaEngine.Resources.ResourceManager content)
        {
            base.Load(content);

            //Sprite.Scale = 3;
            Sprite.Rotation = Source.BodyRotation + Source.TurretRotation + MathHelper.ToRadians(90);
        }

        public override void ProjectileFired()
        {
            ((SimulationRoom)Room).RegisterProjectileFiering(this, Source);

            base.ProjectileFired();
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
