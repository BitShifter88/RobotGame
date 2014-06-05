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
    public class Projectile : GameObject
    {
        public Projectile(Room room, Tank source, Vector2 position, Vector2 direction, float speed)
            : base(room)
        {
            Source = source;
            SetPosition(position);
            Direction = direction;
            Speed = speed;
        }
        public Tank Source { get; set; }
        public Sprite Sprite { get; set; }
        public Vector2 Direction { get; set; }
        public float Speed { get; set; }
        public Damage Damage { get; set; }

        public override void Update(double dt)
        {
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

        private void CheckShieldCollision()
        {
            GameRoom gameRoom = (GameRoom)Room;

            foreach (Shield s in gameRoom.Shields)
            {
                if (s.CheckCollision(Sprite))
                {
                    OnCollisionWithShield(s);
                }
            }
        }

        protected virtual void OnCollisionWithShield(Shield s)
        {

        }

        private void CheckTankCollision()
        {
            GameRoom gameRoom = (GameRoom)Room;

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
    }
}
