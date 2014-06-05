using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Frame.Network.Common;
using System.IO;
using System.Threading;
using Macalania.Robototaker.Log;

namespace Frame.Network.Server
{
    public enum AirUdpProt : byte
    {
        Unsafe = 0,
        Safe = 1,
        HandShacke = 2,
        Ping = 3,
        Disconnected = 4,
    }

    class DatagramAndEndPoint
    {
        public byte[] Datagram { get; set; }
        public IPEndPoint EndPoint { get; set; }
    }

    public class ServerUdp
    {
        int _pingTimeout;

        public delegate void ClosedConnectionEventHandler(Object sender, UdpClientConnectionClosedEventArgs e);
        public event ClosedConnectionEventHandler ClosedConnection;

        public delegate void NewUdpMessageReceivedEventHandler(Object sender, NewUdpClientMessageReceivedEventArgs e);
        public event NewUdpMessageReceivedEventHandler NewUdpMessageReceived;

        public delegate void NewUdpConnectionEventHandler(Object sender, NewUdpClientConnectionEventArgs e);
        public event NewUdpConnectionEventHandler NewUdpConnection;

        UdpClient _listener;
        Dictionary<int, ClientConnectionUdp> _connections = new Dictionary<int, ClientConnectionUdp>();
        List<int> _connetionsToRemove = new List<int>();
        Thread _pingThread;
        Thread _listenThread;
        bool _closing = false;
        int _port;


        public void StartServer(int port, int timeout)
        {
            _pingTimeout = timeout;
            _port = port;
            _listener = new UdpClient(port);
            _listener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);


            _pingThread = new Thread(new ThreadStart(PingRunner));
            _pingThread.Start();

            _listenThread = new Thread(new ThreadStart(ListenRunner));
            _listenThread.Start();

            ServerLog.E("UDP server started!", LogType.ConnectionStatus);
        }

        public void CloseServer()
        {
            _closing = true;

            _listenThread.Abort();
            _pingThread.Abort();
        }

        private void ListenRunner()
        {
            byte[] recieveBuffer;

            while (_closing == false)
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, _port);
                recieveBuffer = _listener.Receive(ref endPoint);
                Thread t = new Thread(new ParameterizedThreadStart(HandleDatagram));

                t.Start(new DatagramAndEndPoint() { Datagram = recieveBuffer, EndPoint = new IPEndPoint(endPoint.Address, _port) });
            }
        }

        private void PingRunner()
        {
            while (_closing == false)
            {
                Thread.Sleep(1000);
                foreach (KeyValuePair<int, ClientConnectionUdp> con in _connections)
                {
                    if (con.Value.CheckForTimeout() == true)
                    {
                        ServerLog.E("Closing connection " + con.Value.Id + " due to ping timeout!", LogType.ConnectionStatus);
                        CloseConnection(con.Key);
                    }
                    else
                    {
                        con.Value.PingClient();
                    }
                }

                foreach (int id in _connetionsToRemove)
                {
                    _connections.Remove(id);
                }

                _connetionsToRemove.Clear();
            }
        }

        public void SendMessage(Message message, int id, AirUdpProt prot)
        {
            if (_connections.ContainsKey(id))
            {
                _connections[id].SendMessage(message, prot);
            }
            else
            {
                ServerLog.E("Tried to send a message to connection " + id + ", but no such connection exists", LogType.PossibleBug);
            }
        }

        private void CloseConnection(int id)
        {
            _connections[id].Close();

            ClosedConnection(this, new UdpClientConnectionClosedEventArgs(_connections[id]));

            _connetionsToRemove.Add(id);
            ServerLog.E("Connection " + id + " clossed!", LogType.ConnectionStatus);
        }

        private void HandleDatagram(object datagramAndEndpoint)
        {
            IPEndPoint endPoint = ((DatagramAndEndPoint)datagramAndEndpoint).EndPoint;
            byte[] recieveBuffer = ((DatagramAndEndPoint)datagramAndEndpoint).Datagram;

            byte[] header = new byte[5];
            byte[] messageBytes = new byte[recieveBuffer.Length - 5];

            for (int i = 0; i < 5; i++)
            {
                header[i] = recieveBuffer[i];
            }

            for (int i = 5; i < recieveBuffer.Length; i++)
            {
                messageBytes[i - 5] = recieveBuffer[i];
            }

            // Reads the connection id and protocl header of the message
            int senderId;
            AirUdpProt protHeader;
            using (MemoryStream memStream = new MemoryStream(header))
            {
                using (BinaryReader br = new BinaryReader(memStream))
                {
                    protHeader = (AirUdpProt)br.ReadByte();
                    senderId = br.ReadInt32();
                }
            }

            if (protHeader == AirUdpProt.Safe)
            {
                if (_connections.ContainsKey(senderId) == true)
                {
                    Message message = new Message(messageBytes);
                    NewUdpMessageReceived(this, new NewUdpClientMessageReceivedEventArgs(message, _connections[senderId]));
                }
                else
                {
                    ServerLog.E("Unknown message from " + endPoint.ToString(), LogType.Security);
                }
            }
            else if (protHeader == AirUdpProt.Unsafe)
            {
                if (_connections.ContainsKey(senderId) == true)
                {
                    Message message = new Message(messageBytes);
                    NewUdpMessageReceived(this, new NewUdpClientMessageReceivedEventArgs(message, _connections[senderId]));
                }
                else
                {
                    ServerLog.E("Unknown message from " + endPoint.ToString(), LogType.Security);
                }
            }
            else if (protHeader == AirUdpProt.HandShacke)
            {
                ClientConnectionUdp connection = new ClientConnectionUdp(new IPEndPoint(endPoint.Address, senderId));
                ServerLog.E("Client " + connection.Id + " connected!", LogType.ConnectionStatus);
                _connections.Add(connection.Id, connection);
                ReturnHandShacke(connection);
                NewUdpConnection(this, new NewUdpClientConnectionEventArgs(connection));
            }
            else if (protHeader == AirUdpProt.Ping)
            {
                if (_connections.ContainsKey(senderId) == true)
                {
                    Message message = new Message(messageBytes);
                    MessageReader r = new MessageReader();
                    r.SetNewMessage(message, 0);
                    int pingId = r.ReadInt();

                    _connections[senderId].PingAnswer(pingId);
                }
                else
                {
                    ServerLog.E("Unknown message from " + endPoint.ToString(), LogType.Security);
                }
            }
        }

        private void ReturnHandShacke(ClientConnectionUdp connection)
        {
            Message message = new Message();
            message.Write(connection.Id);
            message.Write(connection.Id);

            connection.SendMessage(message, AirUdpProt.HandShacke);
        }
    }
}
