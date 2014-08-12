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

#if SERVER
        public YunaTexture(bool[,] transMap, int width, int height)
        {
            Width = width;
            Height = height;
            _transperencyMap = transMap;
        }
#endif

        public bool[,] GetTransperancyMap()
        {
            return _transperencyMap;
        }

        public YunaTexture(Texture2D xnaTexture, bool[,] transMap)
        {
            _transperencyMap = transMap;
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
