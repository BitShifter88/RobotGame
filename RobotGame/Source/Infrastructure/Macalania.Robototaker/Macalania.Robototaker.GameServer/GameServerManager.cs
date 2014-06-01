using Frame.Network.Common;
using Frame.Network.Server;
using Macalania.Robototaker.Log;
using Macalania.Robototaker.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Macalania.Robototaker.GameServer
{
    public class GameServerManager
    {
        ServerUdp _server;
        Simulation _world;
        List<PotentialPlayer> _potPlayers = new List<PotentialPlayer>();
        Mutex _connectionMutex = new Mutex();

        public void StartServer()
        {
            _server = new ServerUdp();
            _server.NewUdpMessageReceived += new ServerUdp.NewUdpMessageReceivedEventHandler(OnNewMessage);
            _server.NewUdpConnection += new ServerUdp.NewUdpConnectionEventHandler(OnNewConnection);
            _server.ClosedConnection += new ServerUdp.ClosedConnectionEventHandler(OnConnectionClosed);

            _server.StartServer(9999, 10000);
        }

        private void OnNewMessage(object sender, NewUdpClientMessageReceivedEventArgs e)
        {
            MessageReader mr = new MessageReader();
            mr.SetNewMessage(e.Message, 0);

            RobotProt header = (RobotProt)mr.ReadByte();

            if (header == RobotProt.PlayerIdentification)
            {
                PlayerIdentification(mr, e.Connection);
            }

            //if (header == RobotProt.UserInput)
            //{
            //    UserInput(mr, e.Connection);
            //}
        }

        private void PlayerIdentification(MessageReader mr, ClientConnectionUdp connection)
        {
            string username = mr.ReadString();
            string sessionId = mr.ReadString();

            _connectionMutex.WaitOne();
            bool potPlayerFound = false;
            for (int i = 0; i < _potPlayers.Count; i++)
            {
                if (_potPlayers[i].Connection.Id == connection.Id)
                {

                    potPlayerFound = true;
                    break;
                }
            }
            if (potPlayerFound == false)
            {
                ServerLog.E("Player identification did not match a valid connection", LogType.Security);
            }
            _connectionMutex.ReleaseMutex();
        }

        private void UserInput(MessageReader mr, ClientConnectionUdp connection)
        {
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

        private void OnNewConnection(object sender, NewUdpClientConnectionEventArgs e)
        {
            _connectionMutex.WaitOne();
            _potPlayers.Add(new PotentialPlayer() { Connection = e.Connection });
            _connectionMutex.ReleaseMutex();
        }

        private void OnConnectionClosed(object sender, UdpClientConnectionClosedEventArgs e)
        {
            _connectionMutex.WaitOne();

            // If the closed connection was a potential player. The potational player should be removed from the list of potential players
            for (int i = 0 ; i < _potPlayers.Count; i++)
            {
                if (_potPlayers[i].Connection.Id == e.Connection.Id)
                {
                    _potPlayers.Remove(_potPlayers[i]);
                    break;
                }
            }
            _connectionMutex.ReleaseMutex();
        }
    }
}
