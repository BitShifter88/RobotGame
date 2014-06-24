using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Macalania.YunaEngine.Resources
{
    public class YunaImage
    {
        public bool[,] ColMap;
        public int Width;
        public int Height;
    }
    public class ResourceManager
    {
        ContentManager _content;
        Dictionary<string, YunaImage> _images = new Dictionary<string, YunaImage>();
        public string ServerTextureFolder { get; set; }
        static Mutex _contentManagerMutex = new Mutex();

        public ResourceManager(ContentManager content)
        {
            ServerTextureFolder = Application.StartupPath + "\\Content\\";
#if !SERVER
            _content = content;
            _content.RootDirectory = "Content";
#endif
        }

        public YunaTexture LoadYunaTexture(string asset)
        {
#if SERVER
            if (_images.ContainsKey(asset))
            {
                return new YunaTexture(_images[asset].ColMap, _images[asset].Width, _images[asset].Height);
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



                _images.Add(asset, new YunaImage() { ColMap = transMap, Width = img.Width, Height = img.Height });

                btm.Dispose();
                img.Dispose();

                return new YunaTexture(_images[asset].ColMap, _images[asset].Width, _images[asset].Height);
            }
#endif
#if !SERVER
            _contentManagerMutex.WaitOne();
            YunaTexture yt = new YunaTexture(_content.Load<Texture2D>(asset));
            _contentManagerMutex.ReleaseMutex();
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
