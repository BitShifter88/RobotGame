﻿
using Macalania.Robototaker.GameServer;
using Macalania.YunaEngine.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macalania.Robototaker.MainFrame
{
    class Program
    {
        static void Main(string[] args)
        {
            GameInstance gsm = new GameInstance();
            gsm.StartServer();
           
        }
    }
}
