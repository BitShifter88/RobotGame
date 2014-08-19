
using Macalania.Robototaker.GameServer;
using Macalania.Robototaker.MainFrame.Data.Mapping;
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
            CreateDatabase();
            AccountService a = new AccountService();
            a.CreateAccount("steffan888");
        }

        static void CreateDatabase()
        {
            var cfg = new Configuration();
            cfg.Configure();
            cfg.AddAssembly(typeof(Account).Assembly);

            new SchemaExport(cfg).Execute(false, true, false);
        }
    }
}
