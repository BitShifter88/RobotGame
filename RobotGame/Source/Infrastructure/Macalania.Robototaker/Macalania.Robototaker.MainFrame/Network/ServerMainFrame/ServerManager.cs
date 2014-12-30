using Lidgren.Network;
using Macalania.Robototaker.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Robototaker.MainFrame.Network.ServerMainFrame
{
    class ServerManager
    {
        List<AuthorizedServer> _servers = new List<AuthorizedServer>();

        public void AddServer(AuthorizedServer server)
        {
            _servers.Add(server);
        }

        public void ServerDisconnected(NetConnection connection)
        {
            for (int i = 0; i < _servers.Count;i++)
            {
                if (_servers[i].Connection.RemoteUniqueIdentifier == connection.RemoteUniqueIdentifier)
                {
                    ServerLog.E("Server " + _servers[i].Name + " disconnected!", LogType.ConnectionStatus);
                    _servers[i].SignOut();
                    RemoveServer(i);
                }
            }
        }

        private void RemoveServer(int index)
        {
            _servers.RemoveAt(index);
        }

        public List<AuthorizedServer> GetServers()
        {
            return _servers;
        }
    }
}
