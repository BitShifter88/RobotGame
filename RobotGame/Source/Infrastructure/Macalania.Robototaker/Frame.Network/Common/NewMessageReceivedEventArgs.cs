using System;
using System.Collections.Generic;

using System.Text;

namespace Frame.Network.Common
{
    public class NewMessageReceivedEventArgs
    {
        private Message m_Message;
        private long m_SenderID;
        private ConnectionTcp m_SenderConnection;

        public ConnectionTcp SenderConnection
        {
            get { return m_SenderConnection; }
            set { m_SenderConnection = value; }
        }

        public long SenderID
        {
            get { return m_SenderID; }
            set { m_SenderID = value; }
        }

        public Message Message
        {
            get { return m_Message; }
            set { m_Message = value; }
        }

        public NewMessageReceivedEventArgs(Message message, long senderID, ConnectionTcp connection)
        {
            m_Message = message;
            m_SenderID = senderID;
            m_SenderConnection = connection;
        }
    }
}
