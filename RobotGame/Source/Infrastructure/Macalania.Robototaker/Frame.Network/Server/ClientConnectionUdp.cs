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
using Macalania.Robototaker.Log;

namespace Frame.Network.Server
{
    class PingRequest
    {
        static Dictionary<int, byte> _usedIds = new Dictionary<int, byte>();
        static Random _random = new Random();
        public Stopwatch PingStopwatch { get; set; }
        public int Id { get; set; }

        public PingRequest()
        {
            Id = GenerateRandomId();
            PingStopwatch = new Stopwatch();
        }

        static int GenerateRandomId()
        {
            int id = _random.Next(0, int.MaxValue);
            while (_usedIds.ContainsKey(id) == true)
            {
                id = _random.Next(0, int.MaxValue);
            }
            _usedIds.Add(id, 255);
            return id;
        }

        static void ReleaseId(int id)
        {
            _usedIds.Remove(id);
        }

        ~PingRequest()
        {
            ReleaseId(Id);
        }
    }

    public class ClientConnectionUdp
    {
        static Dictionary<int, byte> _usedIds = new Dictionary<int, byte>();
        static Random _random = new Random();
        Dictionary<int, PingRequest> _pings = new Dictionary<int, PingRequest>();

        IPEndPoint _endPoint;
        Socket _sendSocket;

        public int Id { get; set; }
        public int ListenPort { get; set; }
        public int Ping { get; set; }
        public bool Connected { get; set; }
        const int _pingTimeout = 30;
        

        public ClientConnectionUdp(IPEndPoint endPoint)
        {
            Ping = 0;
            Id = -1;
            _endPoint = endPoint;
            _sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            
            Id = GenerateRandomId();
        }

        public void PingClient()
        {
            PingRequest ping = new PingRequest();
            SendPing(ping);
        }

        public void PingAnswer(int id)
        {
            _pings[id].PingStopwatch.Stop();
            Ping = (int)_pings[id].PingStopwatch.ElapsedMilliseconds / 2;

            _pings.Remove(id);
        }

        public bool CheckForTimeout()
        {
            List<int> pingsToRemove = new List<int>();

            foreach (KeyValuePair<int, PingRequest> ping in _pings)
            {
                if (ping.Value.PingStopwatch.ElapsedMilliseconds / 1000 > _pingTimeout)
                {
                    pingsToRemove.Add(ping.Key);
                }
            }

            foreach (int ping in pingsToRemove)
            {
                _pings.Remove(ping);
            }

            if (_pings.Count >= 20)
                return true;
            return false;
        }

        private void SendPing(PingRequest ping)
        {
            _pings.Add(ping.Id, ping);
            ping.PingStopwatch.Start();
           
            Message message = new Message();
            message.Write(Ping);
            message.Write(ping.Id);

            SendMessage(message, AirUdpProt.Ping);
        }

        public void SendMessage(Message message, AirUdpProt prot)
        {
            if (_sendSocket == null)
            {
                ServerLog.E("Tried to send message with a closed connection!", LogType.PossibleBug);
                return;
            }
            byte[] sendBuffer = new byte[message.Data.Length + 1];

            sendBuffer[0] = (byte)prot;

            Array.Copy(message.Data, 0, sendBuffer, 1, message.Data.Length);
            //Thread.Sleep(50);
            _sendSocket.SendTo(sendBuffer, _endPoint);
        }

        //private void RespondPing()
        //{
        //    Message message = new Message();
        //    message.Write(Id);
        //    SendMessage(message, AirUdpProt.Ping);
        //}

        public void Close()
        {
            Message message = new Message();
            SendMessage(message, AirUdpProt.Disconnected);

            Thread.Sleep(1);

            _sendSocket.Close();
            _sendSocket.Dispose();
            _sendSocket = null;
        }

        static int GenerateRandomId()
        {
            int id = _random.Next(0, int.MaxValue);
            while (_usedIds.ContainsKey(id) == true)
            {
                id = _random.Next(0, int.MaxValue);
            }
            _usedIds.Add(id, 255);
            return id;
        }

        static void ReleaseId(int id)
        {
            _usedIds.Remove(id);
        }

        ~ClientConnectionUdp()
        {
            ReleaseId(Id);
        }
    }
}
