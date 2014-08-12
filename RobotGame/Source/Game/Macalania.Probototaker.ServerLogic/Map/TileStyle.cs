using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Resources;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Map
{
    public enum TileStyleType : byte
    {
        Jungle = 0,
    }
    public class TileStyle
    {
        public TileStyleType Type { get; set; }
        public Sprite TileSet { get; set; }

        public TileStyle(TileStyleType type)
        {
            Type = type;
        }

        public static TileStyle CreateFromType(TileStyleType type)
        {
            if (type == TileStyleType.Jungle)
                return new JungleStyle();
            throw new Exception("Style not found");
        }

        public virtual Rectangle GetSource(byte tileType)
        {
            throw new Exception("Should be implemented");
        }

        public virtual void Load(ResourceManager content)
        {

        }
    }
}
