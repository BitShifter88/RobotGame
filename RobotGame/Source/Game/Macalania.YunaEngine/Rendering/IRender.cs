using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.YunaEngine.Rendering
{
    public interface IRender
    {
        void Begin(Camera camera);
        void Draw(YunaTexture texture, Vector2 position, Rectangle? source, Color color, float rotation, Vector2 origin, float scale, float depthLayer);
        void End();
    }
}
