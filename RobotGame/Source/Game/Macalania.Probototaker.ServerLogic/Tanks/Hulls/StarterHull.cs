using Macalania.YunaEngine.Graphics;
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
        public StarterHull()
        {
            StoredPower = 1000;
        }
        public override void Load(ContentManager content)
        {
            Sprite = new Sprite(content.Load<Texture2D>("Textures/Tanks/Hulls/hullStarter"));
            base.Load(content);
        }
    }
}
