using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Frame.Network.Common;
using Frame.Network.Server;
using System.Threading;
using System.Diagnostics;

namespace Frame.Network.Client
{
   public class ClientUdp
    {
        public delegate void NewUdpMessageReceivedEventHandler(Object sender, NewUdpServerMessageReceivedEventArgs e);
        public event NewUdpMessageReceivedEventHandler NewUdpMessageReceived;

        public delegate void ClosedConnectionEventHandler(Object sender, UdpServerConnectionClosedEventArgs e);
        public event ClosedConnectionEventHandler ClosedConnection;

        ServerConnectionUdp _connection;
        IPEndPoint _serverEndPoint;

        public ServerConnectionUdp Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        public bool Connect(string ip, int port, int connectionTimeout)
        {
            _serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            _connection = new ServerConnectionUdp(_serverEndPoint);

            _connection.ClosedConnection += new ServerConnectionUdp.ClosedConnectionEventHandler(OnUdpConnectionClosed);
            _connection.NewUdpMessageReceived += new ServerConnectionUdp.NewUdpMessageReceivedEventHandler(OnNewMessageRecieved);

            _connection.EnableListening(port);
            SendHandShacke();

            Stopwatch timeout = new Stopwatch();
            timeout.Start();

            while (_connection.Connected == false)
            {
                if (timeout.ElapsedMilliseconds / 1000 >= connectionTimeout)
                {
                    return false;
                }
                Thread.Sleep(10);
            }

            if (_connection.Id == -1)
                return false;

            return true;
        }

        public void SendMessage(Message message, AirUdpProt prot)
        {
            _connection.SendMessage(message, prot);
        }

        public void CloseConnection()
        {
            _connection.Close();
        }

        private void SendHandShacke()
        {
            Message message = new Message();
            message.Write((int)_connection.ListenPort);
            _connection.SendMessage(message, AirUdpProt.HandShacke);
        }

        private void OnNewMessageRecieved(object sender, NewUdpServerMessageReceivedEventArgs e)
        {
            NewUdpMessageReceived(sender, e);
        }

        private void OnUdpConnectionClosed(object sender, UdpServerConnectionClosedEventArgs e)
        {
            ClosedConnection(sender, e);
        }
    }
}
