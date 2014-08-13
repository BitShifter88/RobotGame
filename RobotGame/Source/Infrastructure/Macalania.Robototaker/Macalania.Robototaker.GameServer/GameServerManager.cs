using Lidgren.Network;
using Macalania.Robototaker.Log;
using Macalania.Robototaker.Protocol;
using Macalania.YunaEngine.Resources;
using Macalania.YunaEngine.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Macalania.Robototaker.GameServer
{
    class GameServerManager : GameLoop
    {
        Dictionary<long, GameInstance> _instances = new Dictionary<long, GameInstance>();
        long _idCounter = 0;

        ResourceManager _content;

        NetServer Server;
        NetPeerConfiguration Config;
        private Thread _messageThread;

        private bool _stop = false;


        public GameServerManager()
        {
            _content = new ResourceManager(null);
                       
            StartGameLoop();
        }

        public void StartServer()
        {
            Config = new NetPeerConfiguration("game");
#if DEBUG
            Config.SimulatedMinimumLatency = 0.015f;
#endif

            Config.Port = 9999;
            Config.MaximumConnections = 200;
            Config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            Server = new NetServer(Config);

            // Start it
            Server.Start();

            _messageThread = new Thread(new ThreadStart(MessageThreadMethod));
            _messageThread.Start();
        }

        protected override void Update(double dt)
        {
            foreach (KeyValuePair<long, GameInstance> instance in _instances)
            {
                instance.Value.Update(dt);
            }

            base.Update(dt);
        }


        public GameInstance CreateNewGameInstance()
        {
            GameInstance gi = new GameInstance();
            gi.StartGame(_content, Server);
            _instances.Add(GetNextId(), gi);

            return gi;
        }

        private long GetNextId()
        {
            _idCounter++;
            return _idCounter;
        }

        public void StopServer()
        {
            _stop = true;
        }

        private void MessageThreadMethod()
        {
            NetIncomingMessage inc;

            while (_stop == false)
            {
                if ((inc = Server.ReadMessage()) != null)
                {
                    // Theres few different types of messages. To simplify this process, i left only 2 of em here
                    switch (inc.MessageType)
                    {
                        case NetIncomingMessageType.ConnectionApproval:
                            {
                                inc.SenderConnection.Approve();
                                string username = inc.ReadString();
                                string sessionId = inc.ReadString();

                                _instances.FirstOrDefault().Value.OnPlayerIdentified(inc.SenderConnection, username, sessionId);
                            }
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            {
                                NetConnectionStatus status = (NetConnectionStatus)inc.ReadByte();
                                if (status == NetConnectionStatus.Disconnected)
                                {
                                    _instances.FirstOrDefault().Value.OnConnectionClosed(inc.SenderConnection);
                                }
                            }
                            break;
                        case NetIncomingMessageType.Data:
                            {
                                _instances.FirstOrDefault().Value.HandleData(inc);
                            }
                            break;
                        case NetIncomingMessageType.WarningMessage:
                            {
                                ServerLog.E(inc.ReadString(), LogType.ConnectionStatus);
                            }
                            break;
                        case NetIncomingMessageType.Error:
                            {
                                ServerLog.E(inc.ReadString(), LogType.ConnectionStatus);
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

                    Server.Recycle(inc);
                }
                else
                    Thread.Sleep(1);
                Thread.Sleep(0);
            }
        }
    }
}
