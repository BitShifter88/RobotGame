using Macalania.Probototaker.Tanks;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Input;
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
            float mouseXOfset = Viewport.Width / 2 - MouseInput.X;
            float mouseYOfset = Viewport.Height / 2 - MouseInput.Y;

            if (MouseInput.IsRightMousePressed())
                Position = new Vector2(-_tank.Position.X + mouseXOfset,  -_tank.Position.Y + mouseYOfset);
            else
                Position = new Vector2(-_tank.Position.X , -_tank.Position.Y );

            int diff = MouseInput.GetScrollWheelDifference();

            Zoom -= (float)diff / 1000f;

            if (Zoom > 1)
                Zoom = 1;
            if (Zoom < 0.5f)
                Zoom = 0.5f;

            base.Update(dt);
        }
    }
}
