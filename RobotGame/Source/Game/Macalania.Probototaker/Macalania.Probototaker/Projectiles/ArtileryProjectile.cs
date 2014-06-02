using Macalania.Probototaker.Tanks;
using Macalania.YunaEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Projectiles
{
    class ArtileryProjectile : Projectile
    {
        public bool Flying { get; set; }

        public ArtileryProjectile(Tank tankSource, Vector2 position, Vector2 direction, float speed)
            : base(tankSource, position, direction, speed)
        {

        }

        public void Ignite()
        {
            Flying = true;
        }

        public override void Update(double dt)
        {
            if (Flying)
            {
                base.Update(dt);
                Speed += (float)dt * 0.003f;
            }
            else
                Sprite.Position = Position;
        }

        public override void Load(ContentManager content)
        {
            Sprite = new Sprite(content.Load<Texture2D>("Textures/Projectiles/artileryProjectile"));
            Sprite.SetOriginCenter();
            Sprite.DepthLayer = 0.25f;
            base.Load(content);
        }
    }
}
