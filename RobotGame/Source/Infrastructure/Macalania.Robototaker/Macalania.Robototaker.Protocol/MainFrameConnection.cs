﻿using Lidgren.Network;
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

    public enum QueStatus
    {
        Waiting,
        WaitingForOtherPlayers,
        InGame,
    }

    public class MainFrameConnection
    {
        NetClient _client;
        int _authenticationTimeout = 10000;
        MainFrameMessageParser _parser;
        bool _stop = false;
        Thread _messageThread;

        public int SessionId { get; set; }
        public LoginStatus LoggedIn { get; set; }
        public QueStatus QueStatus { get; set; }
        public short QuedGameId { get; set; }
        public string GameServerIp { get; set; }

        public bool Connect(MainFrameMessageHandlerBase messageHandler)
        {
            QueStatus = Protocol.QueStatus.Waiting;
            LoggedIn = LoginStatus.NotLoggedIn;
            _parser = new MainFrameMessageParser(messageHandler);
            Console.WriteLine("Connecting to server...");

            NetPeerConfiguration Config = new NetPeerConfiguration("game");

            _client = new NetClient(Config);

            _client.Start();

            _client.Connect("127.0.0.1", 9998);

            if (WaitForApproval() == false)
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

        public void HostingSuccess(short gameId, string ip)
        {
            QuedGameId = gameId;
            GameServerIp = ip;
            QueStatus = Protocol.QueStatus.InGame;
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

        public LoginStatus WaitForLogin(int timeout)
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

        public void JoinQue()
        {
            NetOutgoingMessage message = _client.CreateMessage();
            message.Write((byte)MainFrameProt.JoinQue);
            message.Write(SessionId);
            _client.SendMessage(message, NetDeliveryMethod.ReliableUnordered);
        }

        public void LeaveQue()
        {
            NetOutgoingMessage message = _client.CreateMessage();
            message.Write((byte)MainFrameProt.LeaveQue);
            message.Write(SessionId);
            _client.SendMessage(message, NetDeliveryMethod.ReliableUnordered);
        }

        internal void ReadyForGame()
        {
            NetOutgoingMessage message = _client.CreateMessage();
            message.Write((byte)MainFrameProt.AskIfReadyForGame);
            message.Write(SessionId);
            message.Write(QuedGameId);
            message.Write(true);
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
                                else if (header == MainFrameProt.AskIfReadyForGame)
                                {
                                    _parser.OnAskIfReady(mr);
                                }
                                else if (header == MainFrameProt.GameHostSuccess)
                                {
                                    _parser.OnGameHostSuccess(mr);
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

        private bool WaitForApproval()
        {
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

            if (s.Elapsed.TotalMilliseconds >= _authenticationTimeout)
                return false;
            return true;
        }
    }
}
