using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Macalania.Robototaker.Protocol
{
    public class MainFrameConnection
    {
        NetClient _client;
        int _authenticationTimeout = 5000;

        public bool Connect()
        {
            Console.WriteLine("Connecting to server...");

            NetPeerConfiguration Config = new NetPeerConfiguration("game");

            _client = new NetClient(Config);

            _client.Start();

            _client.Connect("127.0.0.1", 9998);

            if (WaitForAuthentication() == false)
            {
                Console.WriteLine("Could not connect to Main Frame!");
                return false;
            }
            else
            {
                Console.WriteLine("Connected to Main Frame!");
                //_messageThread = new Thread(new ThreadStart(ReadMessages));
                //_messageThread.Start();
            }
            return true;
        }

        private bool WaitForAuthentication()
        {
            bool authenticated = false;
            NetIncomingMessage inc;
            Stopwatch s = new Stopwatch();
            s.Start();

            while (s.ElapsedMilliseconds <= _authenticationTimeout)
            {
                if (_client.ConnectionStatus == NetConnectionStatus.Connected)
                    break;
                if ((inc = _client.ReadMessage()) != null)
                {
                    switch (inc.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            {
                                authenticated = true;
                                Console.WriteLine("Recieved auth response");
                            }
                            break;
                        case NetIncomingMessageType.WarningMessage:
                            {
                                Console.WriteLine(inc.ReadString());
                            }
                            break;
                        case NetIncomingMessageType.Error:
                            {
                                Console.WriteLine(inc.ReadString());
                            }
                            break;
                        case NetIncomingMessageType.VerboseDebugMessage:
                            {
                                Console.WriteLine(inc.ReadString());
                            }
                            break;
                        case NetIncomingMessageType.DebugMessage:
                            {
                                Console.WriteLine(inc.ReadString());
                            }
                            break;
                    }
                }
                else
                    Thread.Sleep(1);
            }

            if (s.Elapsed.TotalMilliseconds >= 5000)
                return false;
            return true;
        }
    }
}
