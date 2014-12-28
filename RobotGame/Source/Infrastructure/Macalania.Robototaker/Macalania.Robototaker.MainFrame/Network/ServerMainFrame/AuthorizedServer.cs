using Lidgren.Network;
using Macalania.Robototaker.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Robototaker.MainFrame.Network.ServerMainFrame
{
    class AuthorizedServer
    {
        public NetConnection Connection { get; set; }
        public string Name { get; set; }
        public int MaxCapacity { get; set; }
        public int CurrentCapacity { get; set; }

        public AuthorizedServer(NetConnection connection, string name, int maxCapacity)
        {
            Name = name;
            Connection = connection;
            MaxCapacity = maxCapacity;
        }

        public void SignOut()
        {
            ServerLog.E("Server " + Name + " signed out!", LogType.Security);
        }
    }
}
