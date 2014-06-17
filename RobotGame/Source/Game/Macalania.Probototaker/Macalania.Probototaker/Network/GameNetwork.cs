using Frame.Network.Client;
using Frame.Network.Common;
using Frame.Network.Server;
using Macalania.Probototaker.Rooms;
using Macalania.Probototaker.Tanks;
using Macalania.Robototaker.Protocol;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Network
{
    public class GameNetwork
    {
        ClientUdp _client;
        GameRoom _gameRoom;
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

        public ClientUdp GetClientUdp()
        {
            return _client;
        }

        private bool SetupServerConnection()
        {
            try
            {
                _client = new ClientUdp();

                Console.WriteLine("Trying to connect to game...");
                if (_client.Connect("127.0.0.1", 9999, 5) == true)
                {
                    Console.WriteLine("Connected to game!");
                    Authenticate();
                }
                else
                {
                    Console.WriteLine("Failed to connect to game");
                    return false;
                }

                _client.NewUdpMessageReceived += new ClientUdp.NewUdpMessageReceivedEventHandler(OnNewMessageRecieved);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
            }
            return true;
        }

        private void Authenticate()
        {
            Message m = new Message();
            m.Write(_client.Connection.Id);
            m.Write((byte)RobotProt.PlayerIdentification);
            m.Write("steffan88");
            m.Write("seesionId123456");
            _client.SendMessage(m, AirUdpProt.Unsafe);
        }

        private void OnOtherPlayerInfoMovement(MessageReader mr)
        {
            string sessionId = mr.ReadString();
            float x = mr.ReadFloat();
            float y = mr.ReadFloat();
            float bodyRotation = mr.ReadFloat();
            float bodySpeed = mr.ReadFloat();
            float rotationSpeed = mr.ReadFloat();
            DrivingDirection drivingDir = (DrivingDirection)mr.ReadByte();
            RotationDirection rotationDir = (RotationDirection)mr.ReadByte();
            ushort ping = mr.ReadUshort();

            _gameRoom.OtherPlayerInfoMovement(sessionId, new Vector2(x,y), bodyRotation, bodySpeed, rotationSpeed, drivingDir, rotationDir, ping);
        }

        private void OnAuthenticationResponse(MessageReader mr)
        {
            bool result = mr.ReadBool();

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

        private void OnPlayerCompensation(MessageReader mr)
        {
            float x = mr.ReadFloat();
            float y = mr.ReadFloat();
            float bodyRotation = mr.ReadFloat();

            Vector2 position = new Vector2(x, y);

            _gameRoom.PlayerCompensation(position, bodyRotation, _client.Connection.Ping);
        }

        private void OnNewMessageRecieved(object sender, NewUdpServerMessageReceivedEventArgs e)
        {
            try
            {
                MessageReader mr = new MessageReader();
                mr.SetNewMessage(e.Message, 0);

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
            }
            catch(Exception ee)
            {
                System.Windows.Forms.MessageBox.Show(ee.ToString());
            }

            //if (protocol == AirGameProt.PlayerState)
            //{
            //    PlayerState(mr);
            //}
            //else if (protocol == AirGameProt.OtherPlayerState)
            //{
            //    OtherPlayerState(mr);
            //}
        }
    }
}
