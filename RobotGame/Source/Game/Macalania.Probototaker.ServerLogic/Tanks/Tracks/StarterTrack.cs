using Macalania.YunaEngine.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Tracks
{
    public class StarterTrack : Track
    {
        public override void Load(ContentManager content)
        {
            Sprite = new Sprite(content.Load<Texture2D>("Textures/Tanks/Tracks/tracksStarter"));
            Sprite.DepthLayer = 0.1f;
            base.Load(content);
        }
    }
}
