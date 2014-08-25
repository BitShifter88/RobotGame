using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Macalania.Robototaker.Protocol
{
    public enum LoginStatus
    {
        NotLoggedIn,
        LoggedIn,
        LogginFailed,
        LogginTimedOut,
    }

    public class MainFrameConnection
    {
        public int SessionId { get; set; }
        public LoginStatus LoggedIn { get; set; }
        NetClient _client;
        int _authenticationTimeout = 5000;
        MainFrameMessageParser _parser;
        bool _stop = false;
        Thread _messageThread;

        public bool Connect(MainFrameMessageHandlerBase messageHandler)
        {
            LoggedIn = LoginStatus.NotLoggedIn;
            _parser = new MainFrameMessageParser(messageHandler);
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
                _messageThread = new Thread(new ThreadStart(ReadMessages));
                _messageThread.Start();
                //_messageThread = new Thread(new ThreadStart(ReadMessages));
                //_messageThread.Start();
            }
            return true;
        }

        public void Login(string username, string password)
        {
            LoggedIn = LoginStatus.NotLoggedIn;
            NetOutgoingMessage message = _client.CreateMessage();
            message.Write((byte)MainFrameProt.Login);
            message.Write(username);
            message.Write(password);
            _client.SendMessage(message, NetDeliveryMethod.ReliableUnordered);
        }

        public LoginStatus WaitForLoginResponse(int timeout)
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            while (s.ElapsedMilliseconds < timeout)
            {
                if (LoggedIn == LoginStatus.LoggedIn || LoggedIn == LoginStatus.LogginFailed)
                    return LoggedIn;
            }

            LoggedIn = LoginStatus.LogginTimedOut;

            return LoggedIn;
        }

        public void CreateAccount(string username, string inGameName, string password)
        {
            NetOutgoingMessage message = _client.CreateMessage();
            message.Write((byte)MainFrameProt.CreatePlayer);
            message.Write(username);
            message.Write(inGameName);
            message.Write(password);
            _client.SendMessage(message, NetDeliveryMethod.ReliableUnordered);
        }

        private void ReadMessages()
        {
            while (_stop == false)
            {
                NetIncomingMessage mr;

                if ((mr = _client.ReadMessage()) != null)
                {
                    switch (mr.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            {
                                MainFrameProt header = (MainFrameProt)mr.ReadByte();

                                if (header == MainFrameProt.CreatePlayer)
                                {
                                    _parser.OnCreatePlayerResponse(mr);
                                }
                                else if (header == MainFrameProt.Login)
                                {
                                    _parser.OnLoginResponse(mr);
                                }
                            }
                            break;
                    }
                    _client.Recycle(mr);
                }
                else
                    Thread.Sleep(1);

            }
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
