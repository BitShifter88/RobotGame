using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace Macalania.Probototaker.ImageProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] dirs = Directory.GetDirectories(Application.StartupPath);
            if (dirs.Where(i => i.Contains("Content")).FirstOrDefault() == null)
            {
                Console.WriteLine("Must run in a directory with a Content folder");
                Console.ReadLine();
                return;
            }

            ProcessDir(dirs.Where(i => i.Contains("Content")).FirstOrDefault());
        }

        static void ProcessDir(string dir)
        {
            string[] files = Directory.GetFiles(dir);
            string[] dirs = Directory.GetDirectories(dir);

            foreach (string fil in files)
            {
                ProcessFile(fil);
            }

            foreach (string dirr in dirs)
            {
                ProcessDir(dirr);
            }
        }

        static void ProcessFile(string file)
        {
          
            if (file.EndsWith(".jpg") || file.EndsWith(".png"))
            {
                Console.WriteLine(file);
                Image img = Image.FromFile(file);
                Bitmap btm = new Bitmap(img);

                bool[,] transMap = new bool[btm.Width, btm.Height];

                for (int i = 0; i < btm.Width; i++)
                {
                    for (int j = 0; j < btm.Height; j++)
                    {
                        byte color = btm.GetPixel(i, j).A;
                        if (color == 0)
                            transMap[i, j] = false;
                        else
                            transMap[i, j] = true;
                    }
                }

                using (BinaryWriter bw = new BinaryWriter(new FileStream(file + ".col", FileMode.Create)))
                {
                    bw.Write(btm.Width);
                    bw.Write(btm.Height);

                    for (int i = 0; i < btm.Width; i++)
                    {
                        for (int j = 0; j < btm.Height; j++)
                        {
                            bw.Write(transMap[i,j]);
                        }
                    }
                }

                btm.Dispose();
                img.Dispose();
            }
        }
    }
}
