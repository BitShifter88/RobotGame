using Macalania.Probototaker.Rooms;
using Macalania.Probototaker.Tanks;
using Macalania.YunaEngine.GameLogic;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Rendering;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Effects
{
    class Explosion : GameObject
    {
        
        float _haveExistedIn = 0;
        float _lifeSpan = 200;
        float _damage;
        float _range;

        public Explosion(Room room, Vector2 position, float damage, float range)
            : base(room)
        {
            Position = position;
            _damage = damage;
            _range = range;

            OnExplosion();
        }

        private void OnExplosion()
        {
            List<Tank> tanks = ((GameRoom)Room).GetTanks();

            foreach (Tank tank in tanks)
            {
                float dist = Vector2.Distance(tank.Position, Position);
                if (dist < _range)
                {
                    float damage = (1 - (dist / _range)) * _damage;
                    tank.DamageTank(damage, 0);
                }
            }
        }

        public override void Update(double dt)
        {
            
            _haveExistedIn += (float)dt;
            if (_haveExistedIn >= _lifeSpan)
                DestroyGameObject();
            base.Update(dt);
        }


    }
}
