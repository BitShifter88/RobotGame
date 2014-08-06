using Macalania.Probototaker.Tanks.Turrets;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Resources;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks
{
    public class VisualTurretStyle
    {
        public Sprite MainTexture { get; set; }
        public Sprite CornersLeftTop { get; set; }
        public Sprite CornersRightTop { get; set; }
        public Sprite CornersLeftBottom { get; set; }
        public Sprite CornersRightBottom { get; set; }
        public Sprite Sides { get; set; }
        public Sprite Cluder { get; set; }

        public Rectangle GetSidesSource(BrickType type)
        {
            if (type == BrickType.LeftTop)
            {
                return new Rectangle(1 + 1 * 0 + 0 * 16, 1 + 1 * 0 + 0 * 16, 16, 16);
            }
            if (type == BrickType.RightTop)
            {
                return new Rectangle(1 + 1 * 2 + 2 * 16, 1 + 1 * 0 + 0 * 16, 16, 16);
            }
            if (type == BrickType.RightBottom)
            {
                return new Rectangle(1 + 1 * 2 + 2 * 16, 1 + 1 * 2 + 2 * 16, 16, 16);
            }
            if (type == BrickType.LeftBottom)
            {
                return new Rectangle(1 + 1 * 0 + 0, 1 + 1 * 2 + 2 * 16, 16, 16);
            }
            if (type == BrickType.Top)
            {
                return new Rectangle(1 + 1 * 1 + 1 * 16, 1 + 1 * 0 + 0, 16, 16);
            }
            if (type == BrickType.Bottom)
            {
                return new Rectangle(1 + 1 * 1 + 1 * 16, 1 + 1 * 2 + 2 * 16, 16, 16);
            }
            if (type == BrickType.Left)
            {
                return new Rectangle(1 + 1 * 0 + 0 * 16, 1 + 1 * 1 + 1 * 16, 16, 16);
            }
            if (type == BrickType.Right)
            {
                return new Rectangle(1 + 1 * 2 + 2 * 16, 1 + 1 * 1 + 1 * 16, 16, 16);
            }
            else
            {
                return new Rectangle(0, 0, 0, 0);
            }
        }
    }
}
