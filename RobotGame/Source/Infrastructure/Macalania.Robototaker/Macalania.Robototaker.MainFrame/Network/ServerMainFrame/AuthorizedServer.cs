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
        NetServer _server;

        public NetConnection Connection { get; set; }
        public string Name { get; set; }
        public int MaxCapacity { get; set; }
        public int CurrentCapacity { get; set; }

        public AuthorizedServer(NetConnection connection, NetServer server, string name, int maxCapacity)
        {
            _server = server;
            Name = name;
            Connection = connection;
            MaxCapacity = maxCapacity;
        }

        public void SignOut()
        {
            ServerLog.E("Server " + Name + " signed out!", LogType.Security);
        }

        public void RequestGameHosting(short gameId)
        {
            NetOutgoingMessage message = _server.CreateMessage();
            message.Write((byte)Protocol.InfrastructureProt.RequestGameHosting);
            message.Write(gameId);

            Connection.SendMessage(message, NetDeliveryMethod.ReliableUnordered,0);
        }
    }
}
