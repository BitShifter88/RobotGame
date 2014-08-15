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

            _room.ActivatePlugin(type, connection.RemoteUniqueIdentifier, 0, targetPos);
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
        }
        public void OnPlayerIdentified(NetConnection connection, string username, string sessionId, TankPackage tp)
        {
            _connectionMutex.WaitOne();
            GamePlayer gp = _room.AddPlayer(connection, username, sessionId, _gameServerTankId, tp);
            _gameServerTankId++;

            CreateOtherPlayer(gp);

            _connectionMutex.ReleaseMutex();
            AuthenticationResponse(connection, true);
        }

        public void CreateOtherPlayer(GamePlayer player)
        {

           
            foreach (KeyValuePair<long,GamePlayer> p in _room.Players)
            {
                NetOutgoingMessage message = _server.CreateMessage();
                message.Write((byte)RobotProt.CreateOtherPlayer);
                message.Write((byte)player.Tank.ServerSideTankId);
                message.Write(player.Tank.Position.X);
                message.Write(player.Tank.Position.Y);
                message.Write(player.Tank.BodyRotation);
                player.TankPackage.WriteToMessage(message);

                p.Value.Connection.SendMessage(message, NetDeliveryMethod.ReliableUnordered, 0);
            }
        }


        private void AuthenticationResponse(NetConnection connection, bool success)
        {
            NetOutgoingMessage m = _server.CreateMessage();
            m.Write((byte)RobotProt.PlayerIdentification);
            m.Write(success);
            connection.SendMessage(m, NetDeliveryMethod.ReliableUnordered, 0);
        }

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
