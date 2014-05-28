using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Frame.Network.Server
{
    public class NewUdpClientConnectionEventArgs : EventArgs
    {
        private ClientConnectionUdp m_Connection;

        public ClientConnectionUdp Connection
        {
            get { return m_Connection; }
            set { m_Connection = value; }
        }

        public NewUdpClientConnectionEventArgs(ClientConnectionUdp connection)
        {
            m_Connection = connection;
        }
    }
}
