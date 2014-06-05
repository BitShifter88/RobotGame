using Macalania.Probototaker.Effects;
using Macalania.Probototaker.Network;
using Macalania.Probototaker.Tanks;
using Macalania.YunaEngine;
using Macalania.YunaEngine.Graphics;
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
    public class AttackRocketProjectile : Rocket
    {
        public AttackRocketProjectile(Room room, Tank tankSource, Vector2 position, Vector2 direction, float speed)
            : base(room, tankSource, position, direction, speed)
        {
            Damage = new Damage() { AmorPenetration = 1, ComponentDamage = 20, TankDamage = 30 };
            Imprecition = GameRandom.GetRandomFloat(0.00015f);
            if (GameRandom.GetRandoBool())
                Imprecition = -Imprecition;
        }

        public override void Explode()
        {
            base.Explode();

            //SmallExplosion e = new SmallExplosion(Room, Position);
            //YunaGameEngine.Instance.GetActiveRoom().AddGameObjectWhileRunning(e);
        }

       

        public override void Update(double dt)
        {
            if (Flying)
            {
                base.Update(dt);
                Speed += (float)dt * 0.003f;

                Direction = YunaMath.RotateVector2(Direction, Imprecition * (float)dt);
                Sprite.Rotation += Imprecition * (float) dt;
            }
            else
                Sprite.Position = Position;
        }

        public override void Load(ContentManager content)
        {
            Sprite = new Sprite(content.Load<Texture2D>("Textures/Projectiles/attackRocketProjectile"));
            Sprite.SetOriginCenter();
            Sprite.DepthLayer = 0.25f;
            base.Load(content);
        }
    }
}
