using Macalania.Probototaker.Tanks;
using Macalania.YunaEngine.GameLogic;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Projectiles
{
    class Projectile : GameObject
    {
        public Projectile(Tank source, Vector2 position, Vector2 direction, float speed)
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

        public override void Update(double dt)
        {
            Position += Direction * Speed * (float)dt;
            Sprite.Position = Position;
            base.Update(dt);
        }

        public override void Draw(IRender render, Camera camera)
        {
            Sprite.Draw(render, camera);
        }
    }
}
