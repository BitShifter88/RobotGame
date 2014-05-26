using Macalania.YunaEngine.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Hulls
{
    class StarterHull : Hull
    {
        public void Load(ContentManager content)
        {
            CompSprite = new Sprite(content.Load<Texture2D>("Texture/Tanks/Hulls/hullStarter"));
        }
    }
}
