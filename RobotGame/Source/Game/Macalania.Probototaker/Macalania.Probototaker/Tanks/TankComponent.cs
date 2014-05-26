using Macalania.YunaEngine.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks
{
    class TankComponent
    {
        public Sprite Sprite { get; set; }
        public Vector2 GetDim()
        {
            return new Vector2(Sprite.Texture.Width, Sprite.Texture.Height);
        }
    }
}
