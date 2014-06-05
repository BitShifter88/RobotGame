using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Macalania.YunaEngine.Resources
{
    public class YunaTexture
    {
        public int Width { get; set; }
        public int Height { get; set; }

        private Texture2D _xnaTexture;

        private bool[,] _transperencyMap;

        public YunaTexture(bool[,] transMap)
        {
            _transperencyMap = transMap;
        }

        public YunaTexture(Texture2D xnaTexture)
        {
            _xnaTexture = xnaTexture;
            Width = _xnaTexture.Width;
            Height = _xnaTexture.Height;
        }

        public Texture2D GetXnaTexture()
        {
#if SERVER
            return null;
#endif
#if !SERVER
            return _xnaTexture;
#endif
        }
    }
}
