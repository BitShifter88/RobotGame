using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Frame.Network.Client
{
    public class NewUdpServerConnectionEventArgs : EventArgs
    {
        private ServerConnectionUdp m_Connection;

        public ServerConnectionUdp Connection
        {
            get { return m_Connection; }
            set { m_Connection = value; }
        }

        public NewUdpServerConnectionEventArgs(ServerConnectionUdp connection)
        {
            m_Connection = connection;
        }
    }
}
