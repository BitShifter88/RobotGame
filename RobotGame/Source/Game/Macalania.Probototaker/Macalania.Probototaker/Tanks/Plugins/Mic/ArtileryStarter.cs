using Macalania.YunaEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Plugins.Mic
{
    class ArtileryStarter : Plugin
    {
        public ArtileryStarter()
        {
            Size = 1;
            OriginOfset = new Vector2(0, 30);
        }
        public override void Load(ContentManager content)
        {
             Sprite = new Sprite(content.Load<Texture2D>("Textures/Tanks/Misc/artilery"));
            base.Load(content);
        }
    }
}
