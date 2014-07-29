using Frame.Network.Common;
using Lidgren.Network;
using Macalania.Probototaker.Projectiles;
using Macalania.Probototaker.Rooms;
using Macalania.Probototaker.Tanks;
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

        public bool Start(GameRoom gameRoom)
        {
            _gameRoom = gameRoom;
            if (SetupServerConnection())
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

        private bool SetupServerConnection()
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
            outmsg.Write("steffan88");
            outmsg.Write("session12345");

            _client.Connect("192.168.87.101", 9999, outmsg);

            if (WaitForAuthentication() == false)
            {
                Console.WriteLine("Could not connect to server!");
                return false;
            }
            else
            {
                Console.WriteLine("Connected to server!");
                _messageThread = new Thread(new ThreadStart(ReadMessages));
                _messageThread.Start();
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

        private void ReadMessages()
        {
            NetIncomingMessage mr;

            while (_stop == false)
            {
                if ((mr = _client.ReadMessage()) != null)
                {
                    switch (mr.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            {
                                RobotProt header = (RobotProt)mr.ReadByte();

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

            while (!authenticated && s.Elapsed.TotalMilliseconds <= 5000)
            {
                if ((inc = _client.ReadMessage()) != null)
                {
                    switch (inc.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            {
                                authenticated = true;
                                Authenticated = true;
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

        private void OnAuthenticationResponse(NetIncomingMessage mr)
        {
            bool result = mr.ReadBoolean();

            if (result == true)
            {
                Authenticated = true;
                Console.WriteLine("Authentication Successfull!");
            }
            else
            {
                Console.WriteLine("Authentication Failed!");
            }
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

                Projectile proj = Projectile.CreateProjectile(type, t, new Vector2(posX, posY), new Vector2(dirX, dirY));

                float timeBehind = ((mr.SenderConnection.AverageRoundtripTime * 1000f / 1f));

                proj.SetTimeBehind(timeBehind);

                _gameRoom.AddGameObjectWhileRunning(proj);

                
            }
        }

        
    }
}
