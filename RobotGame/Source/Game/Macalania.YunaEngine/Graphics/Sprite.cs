using Macalania.YunaEngine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.YunaEngine.Graphics
{
    public class Sprite
    {
        public Sprite(Texture2D texture)
        {
            Texture = texture;
            Scale = 1;
            Color = Color.White;
        }

        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public Color Color { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; }
        public float DepthLayer { get; set; }

        public void SetOriginCenter()
        {
            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
        }

        public void Draw(IRender render, Camera camera)
        {
            render.Draw(Texture, Position, new Rectangle(0, 0, Texture.Width, Texture.Height), Color, Rotation, Origin, Scale, DepthLayer);
        }
    }
}
