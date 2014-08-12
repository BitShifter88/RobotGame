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
            _contentManagerMutex.WaitOne();



            bool[,] transMap = null;

            int dimx = -1;
            int dimy = -1;

            if (_images.ContainsKey(asset))
            {
                transMap = _images[asset].ColMap;
            }
            else
            {
                string path = ServerTextureFolder + asset + ".png.col";
                path = path.Replace('\\', Path.DirectorySeparatorChar);
                path = path.Replace('/', Path.DirectorySeparatorChar);


                using (BinaryReader br = new BinaryReader(new FileStream(path, FileMode.Open)))
                {
                    dimx = br.ReadInt32();
                    dimy = br.ReadInt32();

                    transMap = new bool[dimx, dimy];

                    for (int i = 0; i < dimx; i++)
                    {
                        for (int j = 0; j < dimy; j++)
                        {
                            transMap[i, j] = br.ReadBoolean();
                        }
                    }
                }

                _images.Add(asset, new YunaImage() { ColMap = transMap, Width = dimx, Height = dimy });

            }

#if SERVER
            YunaTexture yt = new YunaTexture(transMap, dimx, dimy);
            _contentManagerMutex.ReleaseMutex();
            return yt;
#endif

#if !SERVER
            YunaTexture yt = new YunaTexture(_content.Load<Texture2D>(asset), transMap);
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
