using Macalania.Robototaker.Log;
using Macalania.YunaEngine.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Macalania.Robototaker.GameServer
{
    public static class PreLoader
    {
        static List<string> foundFiles = new List<string>();

        public static void PreLoad(ResourceManager content)
        {
            Stopwatch s = new Stopwatch();
            s.Start();


            string resourceFolder = Path.Combine(Application.StartupPath, "Content");

            ServerLog.E("Preloading resources in folder: " + resourceFolder, LogType.Information);

            FindFilesInFolder(resourceFolder);
            
            foreach (string file in foundFiles)
            {
                content.LoadYunaTexture(file);
            }
            
            s.Stop();
            ServerLog.E("Resources loaded in time: " + s.Elapsed.TotalMilliseconds + "ms", LogType.Information);
        }

        private static void FindFilesInFolder(string folder)
        {
            string[] files = Directory.GetFiles(folder);
            string[] dirs = Directory.GetDirectories(folder);

            foreach (string s in dirs)
                FindFilesInFolder(s);

            foreach (string file in files)
            {
                if (file.EndsWith(".png"))
                {
                    string cutfile = file.Remove(file.Length - 4);
                    cutfile = cutfile.Replace('\\', '/');
                    cutfile = cutfile.Remove(0, cutfile.IndexOf("Content") + 8);
                    foundFiles.Add(cutfile);
                }
            }
        }
    }
}
