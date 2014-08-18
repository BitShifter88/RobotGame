using Macalania.Probototaker.Effects;
using Macalania.Probototaker.Network;
using Macalania.Probototaker.Tanks;
using Macalania.YunaEngine;
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
    public class ArtileryProjectile : Rocket
    {
        public ArtileryProjectile(Room room, Tank tankSource, Vector2 position, Vector2 direction, float maxDist, float speed)
            : base(room, tankSource, position, direction, speed, maxDist, ProjectileType.ArtileryProjectile)
        {

        }

        public override void Ignite(Vector2 originPosition, float flyDistance, Random random)
        {
            Imprecition = GameRandom.GetRandomFloat(0.0005f, random);
            if (GameRandom.GetRandoBool(random))
                Imprecition = -Imprecition;
            base.Ignite(originPosition, flyDistance, random);
        }

        public override void Explode()
        {
            base.Explode();

            SmallExplosion e = new SmallExplosion(Room, Position);
            Room.AddGameObjectWhileRunning(e);
        }

        public override void OnCollisionWithTank(Tank tank, TankComponent component)
        {
            
        }

        public override void Update(double dt)
        {
            if (Fired)
            {
                base.Update(dt);
                Speed += (float)dt * 0.003f;

                Direction = YunaMath.RotateVector2(Direction, Imprecition * (float)dt);
                Sprite.Rotation += Imprecition * (float) dt;
            }
            else
                Sprite.Position = Position;
        }

        public override void Load(ResourceManager content)
        {
            Sprite = new Sprite(content.LoadYunaTexture("Textures/Projectiles/artileryProjectile"));
            Sprite.SetOriginCenter();
            Sprite.DepthLayer = 0.25f;
            base.Load(content);
        }
    }
}
