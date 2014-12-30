using Frame.Network.Common;
using Lidgren.Network;
using Macalania.Probototaker.Projectiles;
using Macalania.Probototaker.Rooms;
using Macalania.Probototaker.Tanks;
using Macalania.Probototaker.Tanks.Plugins;
using Macalania.Robototaker.Protocol;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Macalania.Probototaker.Network
{
    public class GameNetwork
    {
        GameRoom _gameRoom;
        NetClient _client;
        Thread _messageThread;

        bool _stop = false;
        public bool Authenticated { get; set; }
        public short GameId { get; set; }
        int _authenticationTimeout = 5000;

        public bool Start(GameRoom gameRoom, TankPackage tp, string serverIp, short gameId)
        {
            GameId = gameId;
            _gameRoom = gameRoom;
            if (SetupServerConnection(tp, serverIp))
            {
                gameRoom.ReadyGameCommunication();
                return true;
            }
            return false;
        }

        public NetClient GetClientUdp()
        {
            return _client;
        }

        private bool SetupServerConnection(TankPackage tp, string serverIp)
        {
#if VERBOSE
            try
            {
#endif
            Console.WriteLine("Connecting to server...");

            NetPeerConfiguration Config = new NetPeerConfiguration("game");
#if DEBUG
            Config.SimulatedMinimumLatency = 0.015f;
#endif
            _client = new NetClient(Config);

            _client.Start();

            NetOutgoingMessage outmsg = _client.CreateMessage();
            outmsg.Write(GameId);
            outmsg.Write("steffan88");
            outmsg.Write("session12345");
            tp.WriteToMessage(outmsg);

            _client.Connect(serverIp, 9999, outmsg);

            if (WaitForAuthentication() == false)
            {
                Console.WriteLine("Could not connect to server!");
                return false;
            }
            else
            {
                Console.WriteLine("Connected to server!");
                //_messageThread = new Thread(new ThreadStart(ReadMessages));
                //_messageThread.Start();
            }

                //Console.WriteLine("Trying to connect to game...");
                //if (_client.Connect("127.0.0.1", 9999, 5) == true)
                //{
                //    Console.WriteLine("Connected to game!");
                //    Authenticate();
                //}
                //else
                //{
                //    Console.WriteLine("Failed to connect to game");
                //    return false;
                //}

                //_client.NewUdpMessageReceived += new ClientUdp.NewUdpMessageReceivedEventHandler(OnNewMessageRecieved);
                #if VERBOSE
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
            }
            #endif
            return true;
        }

        public void ReadMessages()
        {
            NetIncomingMessage mr;

            //while (_stop == false)
            //{
                if ((mr = _client.ReadMessage()) != null)
                {
                    switch (mr.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            {
                                RobotProt header = (RobotProt)mr.ReadByte();
                                //Console.WriteLine(header);
                                if (header == RobotProt.PlayerIdentification)
                                {
                                    OnAuthenticationResponse(mr);
                                }
                                else if (header == RobotProt.PlayerCompensation)
                                {
                                    OnPlayerCompensation(mr);
                                }
                                else if (header == RobotProt.OtherPlayerInfoMovement)
                                {
                                    OnOtherPlayerInfoMovement(mr);
                                }
                                else if (header == RobotProt.ProjectileInfo)
                                {
                                    OnProjectileInfo(mr);
                                }
                                else if (header == RobotProt.CreateOtherPlayer)
                                {
                                    OnCreateOtherPlayer(mr);
                                }
                                else if (header == RobotProt.FullWorldUpdate)
                                {
                                    OnFullWorldUpdate(mr);
                                }
                                else if (header == RobotProt.PlayerUsesAbility)
                                {
                                    OnPlayerUsesAbility(mr);
                                }
                            }
                            break;
                    }
                    _client.Recycle(mr);
                }
            //    else
            //        Thread.Sleep(1);
            //}
        }

        private void OnPlayerUsesAbility(NetIncomingMessage mr)
        {
            byte tankId = mr.ReadByte();
            PluginType type = (PluginType)mr.ReadByte();
            byte targetTank = mr.ReadByte();
            float x = mr.ReadFloat();
            float y = mr.ReadFloat();
            ushort seed = mr.ReadUInt16();

            Tank tt = null;
            if (_gameRoom.OtherPlayers.ContainsKey(targetTank))
                tt = _gameRoom.OtherPlayers[targetTank].GetTank();
            if (_gameRoom.Player.GetTank().ServerSideTankId == targetTank)
                tt = _gameRoom.Player.GetTank();

            // TODO: UNDTAGELSE: Hvad nu hvis en tank ikke findes?
            if (_gameRoom.Player.GetTank().ServerSideTankId == tankId)
            {
                _gameRoom.Player.GetTank().ActivatePlugin(type, new Vector2(x, y), tt, new Random(seed));
            }
            else
            {
                _gameRoom.OtherPlayers[tankId].GetTank().ActivatePlugin(type, new Vector2(x, y), tt, new Random(seed));
            }
        }

        private bool WaitForAuthentication()
        {
            bool authenticated = false;
            NetIncomingMessage inc;
            Stopwatch s = new Stopwatch();
            s.Start();

            while (!authenticated && s.ElapsedMilliseconds <= _authenticationTimeout)
            {
                if ((inc = _client.ReadMessage()) != null)
                {
                    switch (inc.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            {
                                authenticated = true;
                                Authenticated = true;
                                OnAuthenticationResponse(inc);
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

        private void OnFullWorldUpdate(NetIncomingMessage mr)
        {
            byte playerTankId = mr.ReadByte();
            _gameRoom.Player.GetTank().ServerSideTankId = playerTankId;

            byte playerCount = mr.ReadByte();

            for (int i = 0; i < playerCount; i++)
            {
                byte tankId = mr.ReadByte();
                float x = mr.ReadFloat();
                float y = mr.ReadFloat();
                float bodyRotation = mr.ReadFloat();
                float bodySpeed = mr.ReadFloat();
                float rotationSpeed = mr.ReadFloat();

                byte packed = mr.ReadByte();

                DrivingDirection drivingDir = (DrivingDirection)BytePacker.GetFirst(packed);
                RotationDirection bodyDir = (RotationDirection)BytePacker.GetSecond(packed);
                RotationDirection turretDir = (RotationDirection)BytePacker.GetThird(packed);

                float turretRotation = mr.ReadFloat();

                ushort ping = mr.ReadUInt16();

                TankPackage tp = TankPackage.ReadTankPackage(mr);

                _gameRoom.CreateOtherPlayer(tankId, new Vector2(x, y), bodyRotation, bodySpeed, rotationSpeed, drivingDir, bodyDir, turretDir, turretRotation, tp, ping);
            }
            Console.WriteLine("Full world update " + playerCount);
        }

        private void OnOtherPlayerInfoMovement(NetIncomingMessage mr)
        {
            string sessionId = mr.ReadString();
            byte tankId = mr.ReadByte();
            float x = mr.ReadFloat();
            float y = mr.ReadFloat();
            float bodyRotation = mr.ReadFloat();
            float bodySpeed = mr.ReadFloat();
            float rotationSpeed = mr.ReadFloat();

            byte packed = mr.ReadByte();

            DrivingDirection drivingDir = (DrivingDirection)BytePacker.GetFirst(packed);
            RotationDirection bodyDir = (RotationDirection)BytePacker.GetSecond(packed);
            RotationDirection turretDir = (RotationDirection)BytePacker.GetThird(packed);

            float turretRotation = mr.ReadFloat();

            ushort ping = mr.ReadUInt16();

            Console.WriteLine(bodyRotation);

            _gameRoom.OtherPlayerInfoMovement(sessionId, new Vector2(x,y), bodyRotation, bodySpeed, rotationSpeed, drivingDir, bodyDir, turretDir, turretRotation, ping, tankId);
        }

        private void OnCreateOtherPlayer(NetIncomingMessage mr)
        {
            byte tankId = mr.ReadByte();
            float x = mr.ReadFloat();
            float y = mr.ReadFloat();
            float bodyRotation = mr.ReadFloat();
            float bodySpeed = mr.ReadFloat();
            float rotationSpeed = mr.ReadFloat();

            byte packed = mr.ReadByte();

            DrivingDirection drivingDir = (DrivingDirection)BytePacker.GetFirst(packed);
            RotationDirection bodyDir = (RotationDirection)BytePacker.GetSecond(packed);
            RotationDirection turretDir = (RotationDirection)BytePacker.GetThird(packed);

            float turretRotation = mr.ReadFloat();

            ushort ping = mr.ReadUInt16();

            TankPackage tp = TankPackage.ReadTankPackage(mr);

            _gameRoom.CreateOtherPlayer(tankId, new Vector2(x, y), bodyRotation, bodySpeed, rotationSpeed, drivingDir, bodyDir, turretDir, turretRotation, tp, ping);
        }

        private void OnAuthenticationResponse(NetIncomingMessage mr)
        {
            bool result = mr.ReadBoolean();

            Authenticated = true;
            Console.WriteLine("Authentication Successfull!");
            _gameRoom.GameCommunication.RequestFullWorldUpdate();

        }

        private void OnPlayerCompensation(NetIncomingMessage mr)
        {
            float x = mr.ReadFloat();
            float y = mr.ReadFloat();
            float bodyRotation = mr.ReadFloat();

            Vector2 position = new Vector2(x, y);

            _gameRoom.PlayerCompensation(position, bodyRotation, (int)(_client.Connections[0].AverageRoundtripTime * 1000f / 2f));
        }

        private void OnProjectileInfo(NetIncomingMessage mr)
        {
            byte count = mr.ReadByte();

            for (int i = 0; i < count; i++)
            {
                ProjectileType type = (ProjectileType)mr.ReadByte();
                byte tankId = mr.ReadByte();
                float posX = mr.ReadFloat();
                float posY = mr.ReadFloat();
                float dirX = mr.ReadFloat();
                float dirY = mr.ReadFloat();

                // Lidt skidt... Vi sender nemlig info om skud som spilleren selv har skudt
                if (_gameRoom.OtherPlayers.ContainsKey(tankId) == false)
                    continue;
                Tank t = _gameRoom.OtherPlayers[tankId].GetTank();

                Projectile proj = Projectile.CreateProjectile(type, t, _gameRoom, new Vector2(posX, posY), new Vector2(dirX, dirY));

                float timeBehind = ((mr.SenderConnection.AverageRoundtripTime * 1000f / 1f));

                proj.SetTimeBehind(timeBehind);

                _gameRoom.AddGameObjectWhileRunning(proj);

                
            }
        }

        
    }
}
