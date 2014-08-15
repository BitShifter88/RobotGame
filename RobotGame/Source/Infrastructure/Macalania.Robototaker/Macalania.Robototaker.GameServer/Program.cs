﻿
using Macalania.Robototaker.GameServer;
using Macalania.YunaEngine.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Macalania.Robototaker.MainFrame
{
    class Program
    {
        static void Main(string[] args)
        {
            GameServerManager gsm = new GameServerManager();

            gsm.StartServer();
            gsm.CreateNewGameInstance();
            Thread.Sleep(1000);
            GC.Collect();
            gsm.StartGameLoop();
        }
    }
}
