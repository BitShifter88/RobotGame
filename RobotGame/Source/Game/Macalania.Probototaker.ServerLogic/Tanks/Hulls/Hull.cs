using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Rooms;

namespace Macalania.Probototaker.Tanks.Hulls
{
    public class Hull : TankComponent
    {
        public Hull(Room room)
            : base(room)
        {

        }

        public override void Update(double dt)
        {
            base.Update(dt);
          
            Sprite.Rotation = Tank.BodyRotation;
        }
    }
}
