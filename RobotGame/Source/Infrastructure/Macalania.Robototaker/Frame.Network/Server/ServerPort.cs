using System;
using System.Collections.Generic;

using System.Text;
using System.Net.Sockets;

namespace Frame.Network.Server
{
    class ServerPort
    {
        private int m_Port;
        private Socket m_Socket;

        public Socket Socket
        {
            get { return m_Socket; }
            set { m_Socket = value; }
        }

        public int Port
        {
            get { return m_Port; }
            set { m_Port = value; }
        }

        public ServerPort(int port, Socket socket)
        {
            m_Port = port;
            m_Socket = socket;
        }
    }

}
