
using Macalania.Robototaker.GameServer;
using Macalania.Robototaker.Log;
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
            MainFrameConnection mfc = new MainFrameConnection(gsm);

            while (true)
            {
                if (mfc.ConnectToMainFrame(9997))
                    break;
            }

            mfc.Authorize("Sputnik", "123456");

            

            if (mfc.WaitForAuthorize(5000) != AuthorizedStatus.Authorized)
            {
                ServerLog.E("Could not autorize server!", LogType.Security);
                Console.ReadLine();
                return;
            }
            else
                ServerLog.E("Authorized!", LogType.Security);

            gsm.StartServer();
            gsm.StartGameLoop();

            GC.Collect();
            
        }
    }
}
