﻿using Lidgren.Network;
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
        bool _stop;

        public void Start(int port)
        {
            Config = new NetPeerConfiguration("game");

            Config.Port = port;
            Config.MaximumConnections = 2000;
            Config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            _server = new NetServer(Config);
            _server.Start();

            StartReadingMessages();
            ServerLog.E("GServer started!", LogType.Information);
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

        private void OnCreatePlayer(NetIncomingMessage mr)
        {
            string username = mr.ReadString();
            string ingameName = mr.ReadString();
            string password = mr.ReadString();
            
            bool result = _accountService.CreateAccount(username, ingameName, password);

            RespondOnCreatePlayer(mr.SenderConnection, result);
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
                                inc.SenderConnection.Approve();
                            }
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            {
                                NetConnectionStatus status = (NetConnectionStatus)inc.ReadByte();
                                if (status == NetConnectionStatus.Disconnected)
                                {
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

                                //_instances.FirstOrDefault().Value.HandleData(inc);
                            }
                            break;
                        case NetIncomingMessageType.WarningMessage:
                            {
                                ServerLog.E(inc.ReadString(), LogType.ConnectionStatus);
                            }
                            break;
                        case NetIncomingMessageType.Error:
                            ServerLog.E(inc.ReadString(), LogType.ConnectionStatus);
                            {
                            }
                            break;
                        case NetIncomingMessageType.VerboseDebugMessage:
                            {
                                ServerLog.E(inc.ReadString(), LogType.ConnectionStatus);
                            }
                            break;
                        case NetIncomingMessageType.DebugMessage:
                            {
                                ServerLog.E(inc.ReadString(), LogType.ConnectionStatus);
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
