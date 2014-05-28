using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frame.Network.Common;

namespace Frame.Network.Server
{
    public class NewUdpClientMessageReceivedEventArgs : EventArgs
    {
        private Message m_Message;
        private ClientConnectionUdp m_Connection;

        public ClientConnectionUdp Connection
        {
            get { return m_Connection; }
            set { m_Connection = value; }
        }

        public Message Message
        {
            get { return m_Message; }
            set { m_Message = value; }
        }

        public NewUdpClientMessageReceivedEventArgs(Message message, ClientConnectionUdp connection)
        {
            m_Connection = connection;
            m_Message = message;
        }
    }
}
