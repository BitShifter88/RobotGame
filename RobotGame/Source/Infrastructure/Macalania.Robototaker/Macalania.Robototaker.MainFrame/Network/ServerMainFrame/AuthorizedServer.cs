using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Robototaker.MainFrame.Network.ServerMainFrame
{
    class AuthorizedServer
    {
        public NetConnection Connection { get; set; }

        public AuthorizedServer(NetConnection connection)
        {
            Connection = connection;
        }
    }
}
