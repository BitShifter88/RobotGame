﻿using System;
using System.Collections.Generic;

using System.Text;

namespace Frame.Network.Common
{
    public class ConnectionClosedEventArgs : EventArgs
    {
        private ConnectionTcp m_Connection;

        public ConnectionTcp Connection
        {
            get { return m_Connection; }
            set { m_Connection = value; }
        }

        public ConnectionClosedEventArgs(ConnectionTcp connection)
        {
            m_Connection = connection;
        }
    }
}
