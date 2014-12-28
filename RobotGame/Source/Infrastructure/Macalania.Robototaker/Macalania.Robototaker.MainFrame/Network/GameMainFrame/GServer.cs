using Lidgren.Network;
using Macalania.Robototaker.Log;
using Macalania.Robototaker.MainFrame.Services;
using Macalania.Robototaker.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Macalania.Robototaker.MainFrame.Network.GameMainFrame
{
    class GServer
    {
        public NetPeerConfiguration Config { get; set; }
        NetServer _server;
        Thread _messageThread;
        AccountService _accountService = new AccountService();
        SessionManager _sessionManager;
        GameManager _gameManager;
        QueManager _queManager;

        Thread _serverLoop;

        bool _stop;

        public void Start(int port)
        {
            Config = new NetPeerConfiguration("game");

            Config.Port = port;
            Config.MaximumConnections = 2000;
            Config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            _server = new NetServer(Config);
            _server.Start();

            _sessionManager = new SessionManager(_server);
            _gameManager = new GameManager(_server);
            _queManager = new QueManager(_gameManager, _server);


            StartReadingMessages();
            ServerLog.E("GServer started on port " + port + "!", LogType.Information);
        }

        private void StartServerLoop()
        {
            _serverLoop = new Thread(new ThreadStart(ServerLoop));
            _serverLoop.Start();
        }

        private void ServerLoop()
        {
            while(_stop == false)
            {
                Thread.Sleep(500);
                _queManager.MatchMake();
                _gameManager.CheckGames();
            }
        }

        public NetServer GetServer()
        {
            return _server;
        }

        public void Stop()
        {
            _stop = true;
        }

        private void StartReadingMessages()
        {
            _messageThread = new Thread(new ThreadStart(ReadMessages));
            _messageThread.Start();
        }

        private void RespondOnCreatePlayer(NetConnection connection, bool result)
        {
            NetOutgoingMessage m = _server.CreateMessage();
            m.Write((byte)MainFrameProt.CreatePlayer);
            m.Write(result);
            connection.SendMessage(m, NetDeliveryMethod.ReliableUnordered, 0);
        }

        private void OnLogin(NetIncomingMessage mr)
        {
            string username = mr.ReadString();
            string password = mr.ReadString();
      
            _sessionManager.LoginAttempt(mr.SenderConnection, username, password);
        }

        private void OnCreatePlayer(NetIncomingMessage mr)
        {
            string username = mr.ReadString();
            string ingameName = mr.ReadString();
            string password = mr.ReadString();
            
            bool result = _accountService.CreateAccount(username, ingameName, password);

            RespondOnCreatePlayer(mr.SenderConnection, result);
        }

        private void OnAskIfReadyForgame(NetIncomingMessage mr)
        {
            bool ready = mr.ReadBoolean();
        }

        private void OnJoinQue(NetIncomingMessage mr)
        {
            int sessionId = mr.ReadInt32();
            PlayerSession session = _sessionManager.GetSession(sessionId);

            _queManager.AddPlayer(session);
        }
        private void OnLeaveQue(NetIncomingMessage mr)
        {
            int sessionId = mr.ReadInt32();
            PlayerSession session = _sessionManager.GetSession(sessionId);

            _queManager.RemovePlayer(session);
        }

        private void OnDisconnect(NetConnection connection)
        {
            _sessionManager.DisconnectedPlayer(connection);
        }


        private void ReadMessages()
        {
            NetIncomingMessage inc;
            while (_stop == false)
            {
                if ((inc = _server.ReadMessage()) != null)
                {
                    switch (inc.MessageType)
                    {
                        case NetIncomingMessageType.ConnectionApproval:
                            {
                                ServerLog.E("Client " + inc.SenderConnection.RemoteUniqueIdentifier + " connected!", LogType.ConnectionStatus);
                                inc.SenderConnection.Approve();

                            }
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            {
                                NetConnectionStatus status = (NetConnectionStatus)inc.ReadByte();
                                if (status == NetConnectionStatus.Disconnected)
                                {
                                    ServerLog.E("Client " + inc.SenderConnection.RemoteUniqueIdentifier + " disconnected!", LogType.ConnectionStatus);
                                    OnDisconnect(inc.SenderConnection);
                                    //_instances.FirstOrDefault().Value.OnConnectionClosed(inc.SenderConnection);
                                }
                            }
                            break;
                        case NetIncomingMessageType.Data:
                            {
                                MainFrameProt header = (MainFrameProt)inc.ReadByte();

                                if (header == MainFrameProt.CreatePlayer)
                                {
                                    OnCreatePlayer(inc);
                                }
                                if (header == MainFrameProt.Login)
                                {
                                    OnLogin(inc);
                                }
                                if (header == MainFrameProt.AskIfReadyForGame)
                                {
                                    OnAskIfReadyForgame(inc);
                                }
                                if (header == MainFrameProt.JoinQue)
                                {
                                    OnJoinQue(inc);
                                }
                                if (header == MainFrameProt.LeaveQue)
                                {
                                    OnLeaveQue(inc);
                                }

                                //_instances.FirstOrDefault().Value.HandleData(inc);
                            }
                            break;
                        case NetIncomingMessageType.WarningMessage:
                            {
                                ServerLog.E(inc.ReadString(), LogType.Lidgren);
                            }
                            break;
                        case NetIncomingMessageType.Error:
                            ServerLog.E(inc.ReadString(), LogType.ConnectionStatus);
                            {
                                ServerLog.E(inc.ReadString(), LogType.Lidgren);
                            }
                            break;
                        case NetIncomingMessageType.VerboseDebugMessage:
                            {
                                ServerLog.E(inc.ReadString(), LogType.Lidgren);
                            }
                            break;
                        case NetIncomingMessageType.DebugMessage:
                            {
                                ServerLog.E(inc.ReadString(), LogType.Lidgren);
                            }
                            break;
                    }

                    _server.Recycle(inc);
                }
                else
                    Thread.Sleep(1);
                Thread.Sleep(0);
            }
        }
    }
}
