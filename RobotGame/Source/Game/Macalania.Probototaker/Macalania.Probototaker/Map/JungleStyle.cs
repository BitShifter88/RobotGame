using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Resources;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Map
{
    public enum JungleTile
    {
        Dirt1 = 0,
        Dirt2 = 1,
        Dirt3 = 2,
        Dirt4 = 3,
    }
    public class JungleStyle : TileStyle
    {
        public JungleStyle() : base(TileStyleType.Jungle)
        {

        }

        public override Rectangle GetSource(byte tileType)
        {
            JungleTile tile = (JungleTile)tileType;

            if (tile == JungleTile.Dirt1)
                return new Rectangle(64 * 0, 64 * 0, 64, 64);
            if (tile == JungleTile.Dirt2)
                return new Rectangle(64 * 1, 64 * 0, 64, 64);
            if (tile == JungleTile.Dirt3)
                return new Rectangle(64 * 1, 64 * 1, 64, 64);
            if (tile == JungleTile.Dirt4)
                return new Rectangle(64 * 0, 64 * 1, 64, 64);

            throw new Exception("No tile found");
        }

        public override void Load(ResourceManager content)
        {
            TileSet = new Sprite(content.LoadYunaTexture("Textures/Tiles/jungle"));
            base.Load(content);
        }
    }
}
