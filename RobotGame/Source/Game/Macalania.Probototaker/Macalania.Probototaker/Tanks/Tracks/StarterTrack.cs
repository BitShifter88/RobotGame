using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Resources;
using Macalania.YunaEngine.Rooms;
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
                public StarterTrack(Room room) : base(room)
        {

        }

        public override void Load(ResourceManager content)
        {
            Sprite = new Sprite(content.LoadYunaTexture("Textures/Tanks/Tracks/tracksStarter"));
            Sprite.DepthLayer = 0.1f;
            base.Load(content);
        }
    }
}
