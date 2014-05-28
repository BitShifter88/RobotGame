using System;
using System.Collections.Generic;

using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Frame.Network.Common;

namespace Frame.Network.Server
{
    class ServerGateTcp
    {
        private List<ServerPort> m_OpenServerPorts = new List<ServerPort>();
        private List<Thread> m_ListenThreads = new List<Thread>();

        public delegate void NewConnectionEventHandler(Object sender, NewConnectionEventArgs e);
        public event NewConnectionEventHandler NewConnection;

        public void OpenPort(int port)
        {
            ServerPort serverPort = null;
            try
            {
                Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                                    ProtocolType.Tcp);
                server.Bind(new IPEndPoint(IPAddress.Any, port));
                server.Listen(16);
                serverPort = new ServerPort(port, server);
                //server.LingerState.Enabled = false;
            }
            catch (Exception e)
            {
            }

            if (serverPort != null)
            {
                m_OpenServerPorts.Add(serverPort);

                Thread listenThread = new Thread(Listen);
                m_ListenThreads.Add(listenThread);
                listenThread.IsBackground = true;
                listenThread.Start(serverPort);
            }
        }

        private void Listen(Object serverPortObject)
        {
            ServerPort serverPort = (ServerPort)serverPortObject;
            bool close = false;
            while (close == false)
            {

                Socket client = serverPort.Socket.Accept(); // Get client connection
                ConnectionTcp connection = new ConnectionTcp(serverPort.Port, client);

                NewConnection(this, new NewConnectionEventArgs(connection));

            }
        }

        public void Close()
        {
            for (int i = 0; i < m_ListenThreads.Count; i++)
            {
                m_ListenThreads[i].Abort();
            }

            m_OpenServerPorts.Clear();
            m_ListenThreads.Clear();
        }
    }
}
