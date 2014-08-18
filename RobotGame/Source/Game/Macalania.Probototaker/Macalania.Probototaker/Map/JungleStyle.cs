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
        Grass = 0,
    }
    public class JungleStyle : TileStyle
    {
        public JungleStyle() : base(TileStyleType.Jungle)
        {

        }

        public override Rectangle GetSource(byte tileType)
        {
            JungleTile tile = (JungleTile)tileType;

            if (tile == JungleTile.Grass)
                return new Rectangle(32 * 2, 32 * 2, 32, 32);

            throw new Exception("No tile found");
        }

        public override void Load(ResourceManager content)
        {
            TileSet = new Sprite(content.LoadYunaTexture("Textures/Tiles/jungle"));
            base.Load(content);
        }
    }
}
