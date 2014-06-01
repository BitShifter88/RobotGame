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
    class ShellStarter : Projectile
    {
        public ShellStarter(Tank tankSource, Vector2 position, Vector2 direction, float speed)
            : base(tankSource, position, direction, speed)
        {

        }

        public override void Load(ContentManager content)
        {
            Sprite = new Sprite(content.Load<Texture2D>("Textures/Projectiles/bullet"));
            Sprite.SetOriginCenter();
            base.Load(content);
        }
    }
}
