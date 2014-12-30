using Lidgren.Network;
using Macalania.Robototaker.Log;
using Macalania.Robototaker.MainFrame.Network.GameMainFrame;
using Macalania.Robototaker.MainFrame.Services;
using Macalania.Robototaker.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Macalania.Robototaker.MainFrame.Network.ServerMainFrame
{
    class SServer
    {
        public NetPeerConfiguration Config { get; set; }
        NetServer _server;
        Thread _messageThread;
        Thread _serverLoop;
        bool _stop;
        ServerManager _serverManager;
        WorkScheduler _workScheduler;

        public SServer()
        {
            _serverManager = new ServerManager();
            _workScheduler = new WorkScheduler(_serverManager);
        }

        public void Start(int port)
        {
            Config = new NetPeerConfiguration("game");

            Config.Port = port;
            Config.MaximumConnections = 20;
            Config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            _server = new NetServer(Config);
            _server.Start();
            
            StartReadingMessages();
            StartServerLoop();
            ServerLog.E("Server Server started on port " + port + "!", LogType.Information);
        }

        private void StartServerLoop()
        {
            _serverLoop = new Thread(new ThreadStart(ServerLoop));
            _serverLoop.Start();
        }

        private void ServerLoop()
        {
            while (_stop == false)
            {
                Thread.Sleep(100);
                

            }
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

        private void OnAuthorize(NetIncomingMessage mr)
        {
            string username = mr.ReadString();
            string password = mr.ReadString();
            int capacity = mr.ReadInt32();

            bool success = false;

            if (password == "123456")
            {
                AuthorizedServer auths = new AuthorizedServer(mr.SenderConnection, _server, username, capacity);
                _serverManager.AddServer(auths);
                ServerLog.E("Server " + username + " authorized!", LogType.Security);
                success = true;
            }
            else
            {
                ServerLog.E("Server " + username + " failed to authorize!", LogType.Security);
            }

            RespondAuthorize(mr.SenderConnection, success);
        }

        private void OnRespondRequestGameHosting(NetIncomingMessage mr)
        {
            short gameId = mr.ReadInt16();
            bool success = mr.ReadBoolean();

            if (success == true)
            {
                _workScheduler.ServerHosted(gameId, mr.SenderConnection.RemoteEndPoint.Address.ToString());
            }
        }


        private void OnServerDisconnect(NetConnection connection)
        {
            
            _serverManager.ServerDisconnected(connection);
        }

        private void RespondAuthorize(NetConnection connection, bool success)
        {
            NetOutgoingMessage message = _server.CreateMessage();

            message.Write((byte)InfrastructureProt.Authorize);
            message.Write(success);

            connection.SendMessage(message, NetDeliveryMethod.ReliableUnordered, 0);
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
                                ServerLog.E("Server " + inc.SenderConnection.RemoteUniqueIdentifier + " connected!", LogType.ConnectionStatus);
                                inc.SenderConnection.Approve();
                            }
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            {
                                NetConnectionStatus status = (NetConnectionStatus)inc.ReadByte();
                                if (status == NetConnectionStatus.Disconnected)
                                {
                                    OnServerDisconnect(inc.SenderConnection);
                                    //_instances.FirstOrDefault().Value.OnConnectionClosed(inc.SenderConnection);
                                }
                            }
                            break;
                        case NetIncomingMessageType.Data:
                            {
                                InfrastructureProt header = (InfrastructureProt)inc.ReadByte();

                                if (header == InfrastructureProt.Authorize)
                                {
                                    OnAuthorize(inc);
                                }
                                else if (header == InfrastructureProt.RequestGameHosting)
                                {
                                    OnRespondRequestGameHosting(inc);
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
