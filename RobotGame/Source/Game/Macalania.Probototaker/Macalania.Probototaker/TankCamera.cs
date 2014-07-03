using Macalania.Probototaker.Tanks;
using Macalania.YunaEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker
{
    class TankCamera : Camera
    {
        Tank _tank;

        public TankCamera(Tank tank)
        {
            _tank = tank;
            Viewport = Globals.Viewport;
        }

        public override void Update(float dt)
        {
            Position = new Vector2(_tank.Position.X,  _tank.Position.Y);

            base.Update(dt);
        }
    }
}
