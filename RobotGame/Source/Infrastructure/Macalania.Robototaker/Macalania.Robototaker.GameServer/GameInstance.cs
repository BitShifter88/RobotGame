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

            PreLoader.PreLoad(_content);

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
        public void OnPlayerIdentified(NetConnection connection, string username, string sessionId)
        {
            _connectionMutex.WaitOne();
            _room.AddPlayer(connection, username, sessionId, _gameServerTankId);
            _gameServerTankId++;
            _connectionMutex.ReleaseMutex();
            AuthenticationResponse(connection, true);
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

            //int commandsIdNew = mr.ReadInt();
            //byte count1 = mr.ReadByte();

            //List<UserCommand> commandsNew = ReadCommands(mr, count1);

            //int commandsIdOld = mr.ReadInt();
            //byte count2 = mr.ReadByte();

            //List<UserCommand> commands2Old = ReadCommands(mr, count2);

            ////commands.AngleOfAttack = (AngleOfAttackState)mr.ReadByte();
            ////commands.AngleOfAttackPerc = mr.ReadFloat();

            ////commands.Role = (RoleState)mr.ReadByte();
            ////commands.RolePerc = mr.ReadFloat();

            ////commands.Engine = (EngineState)mr.ReadByte();

            //_world.UserInput(commandsNew, commandsIdNew, commands2Old, commandsIdOld, connection);
        }

        //private static List<UserCommand> ReadCommands(MessageReader mr, int count)
        //{
        //    List<UserCommand> commands = new List<UserCommand>();

        //    for (int i = 0; i < count; i++)
        //    {
        //        UserCommand command = new UserCommand();
        //        byte packed = mr.ReadByte();

        //        command.AngleOfAttack = (AngleOfAttackState)BytePacker.GetFirst(packed);
        //        command.Role = (RoleState)BytePacker.GetSecond(packed);
        //        command.Engine = (EngineState)BytePacker.GetThird(packed);

        //        if ((byte)command.AngleOfAttack != 0)
        //        {
        //            command.AngleOfAttackPerc = mr.ReadByte();
        //        }
        //        if ((byte)command.Role != 0)
        //        {
        //            command.RolePerc = mr.ReadByte();
        //        }

        //        commands.Add(command);
        //    }

        //    return commands;
        //}

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
