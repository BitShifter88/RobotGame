using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Macalania.YunaEngine.Rendering
{
    public class SimpleRender : IRender
    {
        private SpriteBatch _batch;

        public SimpleRender(GraphicsDevice device)
        {
            _batch = new SpriteBatch(device);
        }

        public void Begin()
        {
            _batch.Begin();
        }

        public void Draw(Texture2D texture, Vector2 position, Rectangle? source, Color color, float rotation, Vector2 origin, float scale, float depthLayer)
        {
            _batch.Draw(texture, position, source, color, rotation, origin, scale, SpriteEffects.None, depthLayer);
        }

        public void End()
        {
            _batch.End();
        }
    }
}
