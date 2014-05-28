using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Frame.Network.Server;
using System.Diagnostics;
using System.Threading;
using Frame.Network.Common;

namespace Frame.Network.Client
{
    public class ServerConnectionUdp
    {
        public delegate void ClosedConnectionEventHandler(Object sender, UdpServerConnectionClosedEventArgs e);
        public event ClosedConnectionEventHandler ClosedConnection;

        public delegate void NewUdpMessageReceivedEventHandler(Object sender, NewUdpServerMessageReceivedEventArgs e);
        public event NewUdpMessageReceivedEventHandler NewUdpMessageReceived;

        static Dictionary<int, byte> _usedIds = new Dictionary<int, byte>();
        static Random _random = new Random();

        IPEndPoint _endPoint;
        Socket _sendSocket;
        UdpClient _listener;
        Thread _listenThread;

        public int Id { get; set; }
        public int ListenPort { get; set; }
        public int Ping { get; set; }
        public bool Connected { get; set; }
        internal Stopwatch PingStopwatch { get; set; }

        public ServerConnectionUdp(IPEndPoint endPoint)
        {
            PingStopwatch = new Stopwatch();
            Ping = 0;
            Id = -1;
            _endPoint = endPoint;
            _sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        public void EnableListening(int port)
        {
            int counter = 1;
            bool succes = false;

            while (succes == false)
            {
                try
                {
                    _listener = new UdpClient(port + counter);
                    succes = true;
                }
                catch (Exception e)
                {
                    counter++;
                }
            }
            ListenPort = port + counter;

            Console.WriteLine("Client listen port: " + ListenPort);

            _listenThread = new Thread(new ThreadStart(Listen));
            _listenThread.Start();
        }

        //internal void SendPing()
        //{
        //    PingStopwatch.Reset();
        //    PingStopwatch.Start();

        //    Message message = new Message();
        //    message.Write((byte)AirUdpProt.Ping);
        //    message.Write(Ping);

        //    SendMessage(message, AirUdpProt.Ping);
        //}

        public void SendMessage(Message message, AirUdpProt prot)
        {
            byte[] sendBuffer = new byte[message.Data.Length + 1];

            sendBuffer[0] = (byte)prot;

            Array.Copy(message.Data, 0, sendBuffer, 1, message.Data.Length);
            //Thread.Sleep(50);
            _sendSocket.SendTo(sendBuffer, _endPoint);
        }

        private void Listen()
        {
            while (true)
            {
                IPEndPoint nullEndPoint = new IPEndPoint(_endPoint.Address, _endPoint.Port);
                byte[] recieveBuffer = _listener.Receive(ref nullEndPoint);
                HandleDatagram(recieveBuffer);
            }
        }

        private void HandleDatagram(byte[] recieveBuffer)
        {
            byte[] header = new byte[1];
            byte[] messageBytes = new byte[recieveBuffer.Length - 1];

            for (int i = 0; i < 1; i++)
            {
                header[i] = recieveBuffer[i];
            }

            for (int i = 1; i < recieveBuffer.Length; i++)
            {
                messageBytes[i - 1] = recieveBuffer[i];
            }

            AirUdpProt protHeader = (AirUdpProt)header[0];
            Message message = new Message(messageBytes);

            if (protHeader == AirUdpProt.Unsafe)
            {
                NewUdpMessageReceived(this, new NewUdpServerMessageReceivedEventArgs(message, this));
            }
            else if (protHeader == AirUdpProt.Safe)
            {
                NewUdpMessageReceived(this, new NewUdpServerMessageReceivedEventArgs(message, this));
            }
            else if (protHeader == AirUdpProt.HandShacke)
            {
                MessageReader mr = new MessageReader();
                mr.SetNewMessage(message, 0);
                int clientId = mr.ReadInt();

                Id = clientId;
                Connected = true;
            }
            else if (protHeader == AirUdpProt.Ping)
            {
                MessageReader mr = new MessageReader();
                mr.SetNewMessage(message, 0);
                Ping = mr.ReadInt();
                int pingId = mr.ReadInt();
                RespondPing(pingId);
            }
            else if (protHeader == AirUdpProt.Disconnected)
            {
                Connected = false;
                ClosedConnection(this, new UdpServerConnectionClosedEventArgs(this));
            }
        }

        private void RespondPing(int pingId)
        {
            Message message = new Message();
            message.Write(Id);
            message.Write(pingId);
            SendMessage(message, AirUdpProt.Ping);
        }

        public void Close()
        {
            Message message = new Message();
            SendMessage(message, AirUdpProt.Disconnected);

            Thread.Sleep(1);

            if (_listenThread != null)
                _listenThread.Abort();
        
            _sendSocket.Close();
            _sendSocket.Dispose();
            if (_listener != null)
               _listener.Close();
        }
    }
}
