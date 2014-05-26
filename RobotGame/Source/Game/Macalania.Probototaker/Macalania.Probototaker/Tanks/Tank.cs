using Macalania.Probototaker.Tanks.Hulls;
using Macalania.Probototaker.Tanks.MainGuns;
using Macalania.Probototaker.Tanks.Tracks;
using Macalania.Probototaker.Tanks.Turrets;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks
{
    class Tank
    {
        public Vector2 Position { get; set; }

        public Hull Hull { get; set; }
        public Track Track { get; set; }
        public Turret Turret { get; set; }

        public Vector2 GetDim()
        {
            //Vector2 dim = Vector2.Zero;

            //if (Hull != null)
            //{
            //    dim.X += Hull.Sprite.Position.X
            //}
        }
    }
}
