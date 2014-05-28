using System;
using System.Collections.Generic;

using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Frame.Network.Common;
using Frame.Network.SmartConnect;

namespace Frame.Network.Client
{
    public static class Client
    {
        public static ConnectionTcp Connect(string serverIP, int port)
        {
            try
            {
                Socket socket;
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                                  ProtocolType.Tcp);
                IPEndPoint socketEndPoint = new IPEndPoint(Dns.Resolve(serverIP).AddressList[0], port);
                socket.Connect(socketEndPoint);
                return new ConnectionTcp(port, socket);
            }
            catch
            {
                return null;
            }
            
        }

        public static ConnectionTcp SmartConnect(string serverSmartIP, int port)
        {
            ResolvedSmartIP resolved = IPResolver.Resolve(serverSmartIP);

            return null;
        }
    }
}
