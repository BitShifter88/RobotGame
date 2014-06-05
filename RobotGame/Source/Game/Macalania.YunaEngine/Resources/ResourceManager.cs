using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Macalania.YunaEngine.Resources
{
    public class ResourceManager
    {
        ContentManager _content;
        Dictionary<string, bool[,]> _images = new Dictionary<string, bool[,]>();
        public string ServerTextureFolder { get; set; }

        public ResourceManager(ContentManager content)
        {
            ServerTextureFolder = Application.StartupPath + "\\Content\\";
            _content = content;
            _content.RootDirectory = "Content";
        }

        public YunaTexture LoadYunaTexture(string asset)
        {
#if SERVER
            if (_images.ContainsKey(asset))
            {
                return new YunaTexture(_images[asset]);
            }
            else
            {
                string path = ServerTextureFolder + asset + ".png";
                path = path.Replace('\\', Path.DirectorySeparatorChar);
                path = path.Replace('/', Path.DirectorySeparatorChar);
                Image img = Image.FromFile(path);
                Bitmap btm = new Bitmap(img);

                bool[,] transMap = new bool[btm.Width, btm.Height];

                for (int i = 0; i < btm.Width; i++)
                {
                    for (int j = 0; j < btm.Height; j++)
                    {
                        if (btm.GetPixel(i, j).A == 255)
                            transMap[i, j] = false;
                        else
                            transMap[i, j] = true;
                    }
                }

                btm.Dispose();
                img.Dispose();

                _images.Add(asset, transMap);

                return new YunaTexture(_images[asset]);
            }
#endif
#if !SERVER
            YunaTexture yt = new YunaTexture(_content.Load<Texture2D>(asset));
            return yt;
#endif
        }

        public T Load<T>(string asset)
        {
            return _content.Load<T>(asset);
        }

        public void Unload()
        {
            _content.Unload();
        }
    }
}
