using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.YunaEngine
{
    public class YunaMath
    {
        public static Vector2 RotateVector2(Vector2 point, float radians)
        {
            return Vector2.Transform(point,
            Matrix.CreateRotationZ(radians));
        }
    }
}
