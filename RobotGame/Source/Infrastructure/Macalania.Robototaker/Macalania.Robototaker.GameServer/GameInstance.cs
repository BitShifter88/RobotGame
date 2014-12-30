using Frame.Network.Common;
using Lidgren.Network;
using Macalania.Probototaker.Network;
using Macalania.Probototaker.Tanks;
using Macalania.Probototaker.Tanks.Plugins;
using Macalania.Robototaker.Log;
using Macalania.Robototaker.Protocol;
using Macalania.YunaEngine.Resources;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Macalania.Robototaker.GameServer
{
    class GameInstance
    {
        byte _gameServerTankId = 0;
        
        ServerRoom _room;
        Mutex _connectionMutex = new Mutex();
        NetServer _server;
        ResourceManager _content;

        // The id used to identify the GameInstace. This is necessary for communication since a game server can have multiple gameinstances running
        public short GameId { get; set; }

        public GameInstance(short id)
        {
            GameId = id;
        }

        public void StartGame(ResourceManager content, NetServer server)
        {
            _content = content;
            _server = server;
            
            _room = new ServerRoom(_server);
            _room.Load(_content);

            ServerLog.E("Server started!", LogType.ConnectionStatus);
        }

        private void OnAbilityActivation(NetIncomingMessage mr, NetConnection connection)
        {
            PluginType type = (PluginType)mr.ReadByte();

            Vector2 targetPos = Vector2.Zero;

            if (type == PluginType.ArtileryStart)
            {
                targetPos = new Vector2(mr.ReadFloat(), mr.ReadFloat());
            }

            _room.ActivatePlugin(type, connection.RemoteUniqueIdentifier, 255, targetPos);
        }

        public void HandleData(NetIncomingMessage inc)
        {
            RobotProt header = (RobotProt)inc.ReadByte();

            if (header == RobotProt.PlayerMovement)
            {
                UserInput(inc, inc.SenderConnection);
            }
            if (header == RobotProt.AbilityActivation)
            {
                OnAbilityActivation(inc, inc.SenderConnection);
            }
            if (header == RobotProt.RequestFullWorldUpdate)
            {
                OnRequestFullWorldUpdate(inc);
            }
        }

        public void OnRequestFullWorldUpdate(NetIncomingMessage inc)
        {
            SendWorldUpdate(_room.Players[inc.SenderConnection.RemoteUniqueIdentifier]);
        }

        public void OnPlayerIdentified(NetConnection connection, string username, string sessionId, TankPackage tp)
        {
            _connectionMutex.WaitOne();
            GamePlayer gp = _room.AddPlayer(connection, username, sessionId, _gameServerTankId, tp);
            _gameServerTankId++;

            
            CreateOtherPlayer(gp);
            

            _connectionMutex.ReleaseMutex();
            
            //AuthenticationResponse(connection, true);
        }

        public void SendWorldUpdate(GamePlayer player)
        {
            NetOutgoingMessage message = _server.CreateMessage();
            message.Write((byte)RobotProt.FullWorldUpdate);

            WriteWorldUpdate(message, player.Connection);

            player.Connection.SendMessage(message, NetDeliveryMethod.ReliableUnordered, 0);
        }

        private void WriteWorldUpdate(NetOutgoingMessage m, NetConnection connection)
        {
            m.Write(_room.Players[connection.RemoteUniqueIdentifier].TankId);
            m.Write((byte)(_room.Players.Count - 1));

            foreach (KeyValuePair<long, GamePlayer> p in _room.Players)
            {
                if (p.Key == connection.RemoteUniqueIdentifier)
                    continue;
                WritePlayerInfo(p.Value, m);
            }
        }

        /// <summary>
        /// Sends a message to all other players that player has joined the game
        /// </summary>
        /// <param name="player"></param>
        public void CreateOtherPlayer(GamePlayer player)
        {
            foreach (KeyValuePair<long,GamePlayer> p in _room.Players)
            {
                if (player.Connection.RemoteUniqueIdentifier == p.Key)
                    continue;

                NetOutgoingMessage message = _server.CreateMessage();
                message.Write((byte)RobotProt.CreateOtherPlayer);
                WritePlayerInfo(player, message);

                p.Value.Connection.SendMessage(message, NetDeliveryMethod.ReliableUnordered, 0);
            }
        }

        private static void WritePlayerInfo(GamePlayer player, NetOutgoingMessage message)
        {
            message.Write(player.Tank.ServerSideTankId);
            message.Write(player.Tank.Position.X);
            message.Write(player.Tank.Position.Y);
            message.Write(player.Tank.BodyRotation);

            message.Write(player.Tank.CurrentSpeed);
            message.Write(player.Tank.CurrentRotationSpeed);

            byte packed = BytePacker.Pack((byte)player.Tank.DrivingDir, (byte)player.Tank.BodyDir, (byte)player.Tank.TurretDir, 0);
            message.Write(packed);

            message.Write(player.Tank.TurretRotation);

            message.Write((ushort)(player.Connection.AverageRoundtripTime * 1000f / 2f));


            player.TankPackage.WriteToMessage(message);
        }


        //public NetOutgoingMessage GetAuthenticationResponse(NetConnection connection, bool success)
        //{
        //    NetOutgoingMessage m = _server.CreateMessage();
        //    m.Write(success);
        //    if (success)
        //        WriteWorldUpdate(m, connection);

        //    return m;
        //}

        private void UserInput(NetIncomingMessage mr, NetConnection connection)
        {
            byte pakced = mr.ReadByte();

            DrivingDirection drivingDir = (DrivingDirection)BytePacker.GetFirst(pakced);
            RotationDirection bodyDirection = (RotationDirection)BytePacker.GetSecond(pakced);
            RotationDirection turretDirection = (RotationDirection)BytePacker.GetThird(pakced);
            byte fireringByte = BytePacker.GetFourth(pakced);
            bool firering = false;

            if (fireringByte == 1)
                firering = true;

            float turretRotation = mr.ReadFloat();

            int commandId = mr.ReadInt32();
            byte broadcastCount = mr.ReadByte();

            PlayerMovement pm = new PlayerMovement() { DrivingDir = drivingDir, BodyDir = bodyDirection, TurretDir = turretDirection, TurretRotation = turretRotation, MainGunFirering = firering};

            _room.PlayerMovement(connection.RemoteUniqueIdentifier, commandId, broadcastCount, pm);

        }

        public void Update(double dt)
        {
            _room.Update(dt);
        }

        public void OnConnectionClosed(NetConnection connection)
        {
            _connectionMutex.WaitOne();
            ServerLog.E("Connection closed " + connection.RemoteUniqueIdentifier, LogType.ConnectionStatus);
            _room.DisconnectedPlayer(connection);
            _connectionMutex.ReleaseMutex();
        }
    }
}
