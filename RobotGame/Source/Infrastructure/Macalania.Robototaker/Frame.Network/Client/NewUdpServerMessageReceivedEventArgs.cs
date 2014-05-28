using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frame.Network.Common;

namespace Frame.Network.Client
{
    public class NewUdpServerMessageReceivedEventArgs : EventArgs
    {
        private Message m_Message;
        private ServerConnectionUdp m_Connection;

        public ServerConnectionUdp Connection
        {
            get { return m_Connection; }
            set { m_Connection = value; }
        }

        public Message Message
        {
            get { return m_Message; }
            set { m_Message = value; }
        }

        public NewUdpServerMessageReceivedEventArgs(Message message, ServerConnectionUdp connection)
        {
            m_Connection = connection;
            m_Message = message;
        }
    }
}
