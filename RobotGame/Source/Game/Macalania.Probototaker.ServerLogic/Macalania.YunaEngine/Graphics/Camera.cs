using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.YunaEngine.Graphics
{
    public class Camera
    {
        public Vector2 Position { get; set; }
        public Viewport Viewport { get; set; }
        public float Zoom { get; set; }

        public Camera()
        {
            Zoom = 1;
        }

        public Vector2 ProjectPosition(int x, int y)
        {
            return new Vector2(x - Position.X - (Viewport.Width) / 2, y - Position.Y - (Viewport.Height) / 2);
        }


        public Vector2 ProjectPositionWithZoom(int x, int y)
        {
            return new Vector2(x - Position.X - (Viewport.Width) / 2 * 1 / Zoom, y - Position.Y - (Viewport.Height) / 2 * 1 / Zoom);
        }

        public Matrix GetMatrix()
        {
            Matrix m = Matrix.CreateTranslation(new Vector3(Position.X, Position.Y, 0)) *
     Matrix.CreateRotationZ(0) *
     Matrix.CreateScale(Zoom) *
     Matrix.CreateTranslation(Viewport.Width / 2, Viewport.Height / 2, 0);
            return m;
        }

        public virtual void Update(float dt)
        {

        }
    }
}
