using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Gui;
using Macalania.YunaEngine.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.YunaEngine.Rendering
{
    class GuiRender : IRender
    {
        SpriteBatch _batch;

        public GuiRender(GraphicsDevice device)
        {
            _batch = new SpriteBatch(device);
        }

        public void Begin(Camera camera)
        {
            _batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Resolution.getTransformationMatrix());
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
