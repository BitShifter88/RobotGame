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

        }

        public Matrix GetMatrix()
        {
            Matrix m = Matrix.CreateTranslation(new Vector3(Position.X, Position.Y, 0)) *
     Matrix.CreateRotationZ(0) *
     Matrix.CreateScale(1) *
     Matrix.CreateTranslation(1300 / 2, 900 / 2, 0);
            return m;
        }

        public virtual void Update(float dt)
        {

        }
    }
}
