using Macalania.Probototaker.Effects;
using Macalania.Probototaker.Rooms;
using Macalania.Probototaker.Tanks;
using Macalania.YunaEngine.GameLogic;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Rendering;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Projectiles
{
    public enum ProjectileType : byte
    {
        ShellStarter = 0,
        RocketStarterProjectile = 1,
        AttackRocketProjectile = 2,
        ArtileryProjectile = 3,
    }

    public class Projectile : GameObject
    {
        public Projectile(Room room, Tank source, Vector2 position, Vector2 direction, float speed, float maxDist, ProjectileType type)
            : base(room)
        {
            Source = source;
            SetPosition(position);
            Direction = direction;
            Speed = speed;
            ProjectileType = type;
            MaxDist = maxDist;
        }
        public Tank Source { get; set; }
        public Sprite Sprite { get; set; }
        public Vector2 Direction { get; set; }
        public float Speed { get; set; }
        public Damage Damage { get; set; }
        public ProjectileType ProjectileType { get; set; }
        public bool Fired { get; private set; }
        public double TimeBehindDivided { get; set; }
        private int _smoothCount = 0;
        private double _smoothTimeRate = 50d;
        public float MaxDist { get; set; }

        public override void Update(double dt)
        {
            if (TimeBehindDivided > 0 && _smoothCount <= _smoothTimeRate)
            {
                _smoothCount++;
                dt += TimeBehindDivided;
            }

            UpdatePosition(dt);

            base.Update(dt);

            Sprite.Update(dt);

            CheckTankCollision();
            CheckShieldCollision();
        }

        public override void SetPosition(Vector2 position)
        {
            if (Sprite != null)
                Sprite.Position = position;
            base.SetPosition(position);
        }

        private void CheckTankCollision()
        {
            SimulationRoom gameRoom = (SimulationRoom)Room;

            List<Tank> tanks = gameRoom.Tanks;

            foreach (Tank t in tanks)
            {
                if (Source == t)
                    continue;
                TankComponent collidingComponent = t.IsColliding(Sprite);
                if (collidingComponent != null)
                {
                    OnCollisionWithTank(t, collidingComponent);
                }
            }
        }

        private void CheckShieldCollision()
        {
            SimulationRoom gameRoom = (SimulationRoom)Room;

            foreach (Shield s in gameRoom.Shields)
            {
                if (s.CheckCollision(Sprite))
                {
                    OnCollisionWithShield(s);
                }
            }
        }

        public void SetTimeBehind(double time)
        {
            TimeBehindDivided = time / _smoothTimeRate;
        }

        public virtual void ProjectileFired()
        {
            Fired = true;
        }

        protected virtual void OnCollisionWithShield(Shield s)
        {

        }

  

        private void UpdatePosition(double dt)
        {
            SetPosition(Position+ Direction * Speed * (float)dt);
            Sprite.Position = Position;
        }

        public virtual void OnCollisionWithTank(Tank tank, TankComponent component)
        {
            DestroyGameObject();
            component.Damage(Damage.ComponentDamage);
            tank.DamageTank(Damage.TankDamage, Damage.AmorPenetration);
        }

        public override void Draw(IRender render, Camera camera)
        {
            Sprite.Draw(render, camera);
        }

        public static Projectile CreateProjectile(ProjectileType type, Tank source, Room room, Vector2 position, Vector2 direction)
        {
            if (type == ProjectileType.ShellStarter)
                return new ShellStarter(room, source, position, direction);
            else
                throw new Exception("Unknown projectile. Cannot create");
        }
    }
}
