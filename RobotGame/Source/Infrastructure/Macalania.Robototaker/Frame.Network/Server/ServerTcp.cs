using System;
using System.Collections.Generic;

using System.Text;
using Frame.Network.Common;

namespace Frame.Network.Server
{
    public class ServerTcp
    {
        private ServerGateTcp m_ServerGate;
        private List<ConnectionTcp> m_Connections = new List<ConnectionTcp>();

        public delegate void NewConnectionEventHandler(Object sender, NewConnectionEventArgs e);
        public event NewConnectionEventHandler NewConnection;
        public delegate void ConnectionClosedEventHandler(Object sender, ConnectionClosedEventArgs e);
        public event ConnectionClosedEventHandler ConnectionClosed;

        string m_PassPhrase;
        string m_SaltValue;
        string m_HashAlgorithm;
        int m_PasswordIterations;
        string m_InitVector;
        int m_KeySize;

        bool networkEncryptionEnabled;

        public bool NetworkEncryptionEnabled
        {
            get { return networkEncryptionEnabled; }
        }

        public ServerTcp()
        {
            m_ServerGate = new ServerGateTcp();
            m_ServerGate.NewConnection += new ServerGateTcp.NewConnectionEventHandler(OnNewConnectionFromServerGate);
        }

        public void EnableNetworkEncryption(string passPhrase,
            string saltValue,
            string hashAlgorithm,
            int passwordIterations,
            string initVector,
            int keySize)
        {
            m_PassPhrase = passPhrase;
            m_SaltValue = saltValue;
            m_HashAlgorithm = hashAlgorithm;
            m_PasswordIterations = passwordIterations;
            m_InitVector = initVector;
            m_KeySize = keySize;

            networkEncryptionEnabled = true;
        }

        public void DisableNetworkEncryption()
        {
            m_PassPhrase = "";
            m_SaltValue = "";
            m_HashAlgorithm = "";
            m_PasswordIterations = -1;
            m_InitVector = "";
            m_KeySize = -1;

            networkEncryptionEnabled = false;
        }

        public void StartServer(int port)
        {
            List<int> list = new List<int>();
            list.Add(port);
            this.StartServer(list);
        }

        public void StartServer(List<int> ports)
        {
            for (int i = 0; i < ports.Count; i++)
            {
                m_ServerGate.OpenPort(ports[i]);
            }
        }

        public void SendMessage(Message message, long connectionID)
        {
            RemoveOldConnections();
            for (int i = 0; i < m_Connections.Count; i++)
            {
                if (m_Connections[i].ID == connectionID)
                {
                    m_Connections[i].SendMessage(message);
                    return;
                }
            }
        }

        private void RemoveOldConnections()
        {
            List<ConnectionTcp> connectionsToRemove = new List<ConnectionTcp>();
            for (int i = 0; i < m_Connections.Count; i++)
            {
                if (m_Connections[i].Connected == false)
                {
                    connectionsToRemove.Add(m_Connections[i]);
                    ConnectionClosed(this, new ConnectionClosedEventArgs(m_Connections[i]));
                }
            }
            foreach (ConnectionTcp connection in connectionsToRemove)
            {
                m_Connections.Remove(connection);
            }
        }

        public void SendMessageToAll(Message message)
        {
            RemoveOldConnections();
            for (int i = 0; i < m_Connections.Count; i++)
            {
                m_Connections[i].SendMessage(message);
            }
        }

        public void StopServer()
        {
            m_ServerGate.Close();
            foreach (ConnectionTcp connection in m_Connections)
            {
                connection.Close();
            }
        }

        private void OnNewConnectionFromServerGate(Object sender, NewConnectionEventArgs e)
        {
            if (networkEncryptionEnabled)
                e.Connection.EnableNetworkEncryption(m_PassPhrase, m_SaltValue, m_HashAlgorithm, m_PasswordIterations, m_InitVector, m_KeySize);
            m_Connections.Add(e.Connection);
            NewConnection(this, e);
        }
    }
}
