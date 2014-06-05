
using Macalania.Robototaker.GameServer;
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
            GameServerManager gsm = new GameServerManager();
            gsm.StartServer();

        }
    }
}
