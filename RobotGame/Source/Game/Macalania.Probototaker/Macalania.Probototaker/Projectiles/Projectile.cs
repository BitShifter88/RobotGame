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
    class Projectile : GameObject
    {
        public Projectile(Room room, Tank source, Vector2 position, Vector2 direction, float speed)
            : base(room)
        {
            Source = source;
            Position = position;
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
            Position += Direction * Speed * (float)dt;
            Sprite.Position = Position;

            GameRoom gameRoom = (GameRoom)Room;

            List<Tank> tanks = gameRoom.GetTanks();

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
            base.Update(dt);

            Sprite.Update(dt);
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
