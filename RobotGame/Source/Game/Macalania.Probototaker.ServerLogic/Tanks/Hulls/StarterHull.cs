using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Resources;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Hulls
{
    public class StarterHull : Hull
    {
        public StarterHull(Room room) : base(room)
        {
            StoredPower = 1000;
            StoredHp = 1000;
        }
        public override void Load(ResourceManager content)
        {
            Sprite = new Sprite(content.LoadYunaTexture("Textures/Tanks/Hulls/hullStarter"));
            Sprite.SetOriginCenter();
            Sprite.DepthLayer = 0.1f;
            base.Load(content);
        }
    }
}
