using Macalania.Probototaker.Tanks;
using Macalania.YunaEngine;
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
        Vector2 _mouseClick = Vector2.Zero;
        bool _presedPrev = false;

        public TankCamera(Tank tank)
        {
            _tank = tank;
            Viewport = Globals.Viewport;
        }

        public override void Update(float dt)
        {
            if (MouseInput.IsRightMousePressed() == false)
                _presedPrev = false;

            if (MouseInput.IsRightMousePressed() && _presedPrev == false)
                _mouseClick = new Vector2(MouseInput.X, MouseInput.Y);

            float mouseXOfset = _mouseClick.X - MouseInput.X;
            float mouseYOfset = _mouseClick.Y - MouseInput.Y;

            if (MouseInput.IsRightMousePressed())
                Position = new Vector2(-_tank.Position.X - mouseXOfset,  -_tank.Position.Y - mouseYOfset);
            else
                Position = new Vector2(-_tank.Position.X , -_tank.Position.Y );

            int diff = MouseInput.GetScrollWheelDifference();

            Zoom -= (float)diff / 1000f;

            if (Zoom > 1)
                Zoom = 1;
            if (Zoom < 0.65f)
                Zoom = 0.65f;

            if (MouseInput.IsRightMousePressed())
            {
                _presedPrev = true;
            }

            base.Update(dt);
        }
    }
}
