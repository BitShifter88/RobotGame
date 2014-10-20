using Macalania.Robototaker.GameServer;
using Macalania.Robototaker.Log;
using Macalania.Robototaker.MainFrame.Data.Mapping;
using Macalania.Robototaker.MainFrame.Network.GameMainFrame;
using Macalania.Robototaker.MainFrame.Network.ServerMainFrame;
using Macalania.Robototaker.MainFrame.Services;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
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
            ServerLog.E("Main Frame starting...", LogType.Information);

            GServer gserver = new GServer();
            gserver.Start(9998);

            SServer sserver = new SServer();
            sserver.Start(9997);

            CreateDatabase();

            Console.ReadLine();
            gserver.Stop();

        }

        static void CreateDatabase()
        {
            ServerLog.E("Creating database...", LogType.Data);
            var cfg = new Configuration();
            cfg.Configure();
            cfg.AddAssembly(typeof(Account).Assembly);

            new SchemaExport(cfg).Execute(false, true, false);

            ServerLog.E("Database created!", LogType.Data);
        }
    }
}
