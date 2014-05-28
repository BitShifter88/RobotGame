using System;
using System.Collections.Generic;

using System.Text;
using System.Net.Sockets;
using System.Threading;

using Frame.Network.Security;

namespace Frame.Network.Common
{
    public class ConnectionTcp
    {
        private Socket m_Socket;
        private int m_Port;
        private Thread m_ReadThread;
        private long m_ID;
        private static int idCounter = 0;
        private byte[] m_HeaderBuffer;
        private bool m_Connected;
        private string m_IP;

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

        public bool Connected
        {
            get { return m_Connected; }
        }

        public string IP
        {
            get { return m_IP; }
            set { m_IP = value; }
        }

        public delegate void NewMessageReceivedEventHandler(Object sender, NewMessageReceivedEventArgs e);
        public event NewMessageReceivedEventHandler NewMessageReceived;

        public long ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        public int Port
        {
            get { return m_Port; }
            set { m_Port = value; }
        }

        public Socket Socket
        {
            get { return m_Socket; }
            set { m_Socket = value; }
        }

        public ConnectionTcp(int port, Socket socket)
        {
            socket.ReceiveBufferSize = 4096;
            m_Connected = socket.Connected;
            m_IP = socket.RemoteEndPoint.ToString();
            m_HeaderBuffer = new byte[4];
            m_ID = idCounter;
            idCounter++;

            m_Port = port;
            m_Socket = socket;

            m_ReadThread = new Thread(new ThreadStart(ReadIncommingMessages));
            m_ReadThread.IsBackground = true;
            m_ReadThread.Start();
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

        private void ReadIncommingMessages()
        {
            Thread.Sleep(0); // To make sure that encryption is activated before reading. TODO: This should be fixed
            while(true)
            {
                Message message;
                try
                {
                    m_Socket.Receive(m_HeaderBuffer, 0, 4, SocketFlags.None);
                    int bufferSize = BitConverter.ToInt32(m_HeaderBuffer, 0);

                    byte[] byteBuffer = new byte[bufferSize];
                    while (m_Socket.Available < bufferSize)
                    {
                        Thread.Sleep(0);
                        if (bufferSize > m_Socket.ReceiveBufferSize)
                            m_Socket.ReceiveBufferSize = bufferSize;
                    }
                    m_Socket.Receive(byteBuffer, 0, bufferSize, SocketFlags.None);

                    if (networkEncryptionEnabled)
                    {
                        byteBuffer = Cryptography.Decrypt(byteBuffer, m_PassPhrase, m_SaltValue, m_HashAlgorithm, m_PasswordIterations, m_InitVector, m_KeySize);
                    }

                    message = new Message(byteBuffer);

                    while (NewMessageReceived == null)
                        Thread.Sleep(1);

                    
                }
                catch (Exception e)
                {
                    Close();
                    return;
                }
                NewMessageReceived(this, new NewMessageReceivedEventArgs(message, m_ID, this));
            }
        }

        public void SendMessage(Message message)
        {
            try
            {
                if (networkEncryptionEnabled)
                {
                    byte[] encryptedBytes = Cryptography.Encrypt(message.Data, m_PassPhrase, m_SaltValue, m_HashAlgorithm, m_PasswordIterations, m_InitVector, m_KeySize);
                    byte[] header = BitConverter.GetBytes(encryptedBytes.Length);
                    m_Socket.Send(header, SocketFlags.None);
                    m_Socket.Send(encryptedBytes, SocketFlags.None);
                }
                else
                {
                    byte[] header = BitConverter.GetBytes(message.Data.Length);
                    m_Socket.Send(header, SocketFlags.None);
                    m_Socket.Send(message.Data, SocketFlags.None);
                }

            }
            catch (Exception e)
            {
                Close();
                return;
            }
        }

        public void Close()
        {
            m_Connected = false;
            m_ReadThread.Abort();
            m_Socket.Close();
        }
    }
}
