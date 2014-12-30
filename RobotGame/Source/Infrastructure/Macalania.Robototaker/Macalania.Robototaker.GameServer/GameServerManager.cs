using Lidgren.Network;
using Macalania.Probototaker.Tanks;
using Macalania.Robototaker.Log;
using Macalania.Robototaker.Protocol;
using Macalania.YunaEngine.Resources;
using Macalania.YunaEngine.Rooms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Macalania.Robototaker.GameServer
{
    class GameServerManager : GameLoop
    {
        Dictionary<short, GameInstance> _instances = new Dictionary<short, GameInstance>();
        byte _idCounter = 0;

        ResourceManager _content;

        NetServer Server;
        NetPeerConfiguration Config;

        private bool _stop = false;

        ThreadLoadRecorder _threadLoadRecorder;

        Thread _consoleReadThread;


        public GameServerManager()
        {
            _content = new ResourceManager(null);
            _threadLoadRecorder = new ThreadLoadRecorder();
            PreLoader.PreLoad(_content);
            ShowStats = true;

            _consoleReadThread = new Thread(new ThreadStart(ConsoleRead));
            _consoleReadThread.Start();

            ShowStats = false;
        }

        private void ConsoleRead()
        {
            while(_stop == false)
            {
                string read = Console.ReadLine();
                read = read.ToLower();
                if (read == "exit" || read == "stop")
                {
                    StopServer();
                }
                if (read == "stopconsole" || read == "stopverbose" || read == "noconsole")
                {
                    ShowStats = false;
                    ServerLog.DisableConsole();
                }
                if (read == "showconsole" || read == "console" || read == "startconsole")
                {
                    ShowStats = true;
                    ServerLog.EnableConsole();
                }
            }
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

            //_messageThread = new Thread(new ThreadStart(MessageThreadMethod));
            //_messageThread.Start();
        }

        protected override void Update(double dt)
        {
            CheckForMessages();

            foreach (KeyValuePair<short, GameInstance> instance in _instances)
            {
                instance.Value.Update(dt);
            }

            CheckForMessages();

            base.Update(dt);
        }

        protected override void SecondUpdate()
        {
            base.SecondUpdate();
        }

        public GameInstance CreateNewGameInstance(short gamId)
        {
            GameInstance gi = new GameInstance(gamId);
            gi.StartGame(_content, Server);
            _instances.Add(gamId, gi);

            return gi;
        }

        private byte GetNextId()
        {
            _idCounter++;
            return _idCounter;
        }

        public void StopServer()
        {
            StopGameLoop();
            Thread.Sleep(100);
            _stop = true;
            Server.DiscoverLocalPeers(9999);
        }

        private void MessageThreadMethod()
        {
            //while (_stop == false)
            //{
            //    CheckForMessages();
            //}
        }

        private void CheckForMessages()
        {
            if (Server == null)
                return;
            NetIncomingMessage inc;
            if ((inc = Server.ReadMessage()) != null)
            {
                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.ConnectionApproval:
                        {
                            inc.SenderConnection.Approve();
                            short gameId = inc.ReadInt16();
                            string username = inc.ReadString();
                            string sessionId = inc.ReadString();
                            TankPackage tp = TankPackage.ReadTankPackage(inc);

                            _instances[gameId].OnPlayerIdentified(inc.SenderConnection, username, sessionId, tp);
                        }
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        {
                            short gameId = inc.ReadInt16();
                            NetConnectionStatus status = (NetConnectionStatus)inc.ReadByte();
                            if (status == NetConnectionStatus.Disconnected)
                            {
                                _instances[gameId].OnConnectionClosed(inc.SenderConnection);
                            }
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        {
                            short gameId = inc.ReadInt16();
                            _instances[gameId].HandleData(inc);
                        }
                        break;
                    case NetIncomingMessageType.WarningMessage:
                        {
                            ServerLog.E(inc.ReadString(), LogType.Lidgren);
                        }
                        break;
                    case NetIncomingMessageType.Error:
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

                Server.Recycle(inc);
            }
            //else
            //    Thread.Sleep(1);
            //Thread.Sleep(0);
        }
    }
}
