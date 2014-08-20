using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Macalania.YunaEngine.Resources;
using Macalania.YunaEngine.Graphics;

namespace Macalania.YunaEngine.Rendering
{
    public class SimpleRender : IRender
    {
        private SpriteBatch _batch;

        public SimpleRender(GraphicsDevice device)
        {
            _batch = new SpriteBatch(device);
        }

        public void Begin(Camera camera)
        {
            _batch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, camera.GetMatrix());
        }

        public void Draw(YunaTexture texture, Vector2 position, Rectangle? source, Color color, float rotation, Vector2 origin, float scale, float depthLayer)
        {
            _batch.Draw(texture.GetXnaTexture(), position, source, color, rotation, origin, scale, SpriteEffects.None, depthLayer);
        }

        public void Draw(YunaTexture texture, Rectangle destination, Color color)
        {
            _batch.Draw(texture.GetXnaTexture(), destination, color);
        }

        public void End()
        {
            _batch.End();
        }
    }
}
