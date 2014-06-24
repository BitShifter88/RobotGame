using Macalania.Probototaker.Tanks;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Resources;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Projectiles
{
    public class RocketStarterProjectile : Rocket
    {
        public RocketStarterProjectile(Room room, Tank tankSource, Vector2 position, Vector2 direction, float speed)
            : base(room, tankSource, position, direction, speed, ProjectileType.RocketStarterProjectile)
        {

        }

        public override void Update(double dt)
        {
            if (Fired)
            {
                base.Update(dt);
                Speed += (float)dt * 0.003f;
            }
            else
                Sprite.Position = Position;
        }

        public override void Load(ResourceManager content)
        {
            Sprite = new Sprite(content.LoadYunaTexture("Textures/Projectiles/rocketStarterProjectile"));
            Sprite.SetOriginCenter();
            Sprite.DepthLayer = 0.25f;
            base.Load(content);
        }
    }
}
