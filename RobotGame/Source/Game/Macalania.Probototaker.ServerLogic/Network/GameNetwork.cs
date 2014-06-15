using Frame.Network.Client;
using Frame.Network.Common;
using Frame.Network.Server;
using Macalania.Robototaker.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Network
{
    public class GameNetwork
    {
        ClientUdp _client;

        public void Start()
        {
            SetupServerConnection();
        }

        public ClientUdp GetClientUdp()
        {
            return _client;
        }

        private void SetupServerConnection()
        {
            _client = new ClientUdp();

            Console.WriteLine("Trying to connect to game...");
            if (_client.Connect("212.10.156.150", 9999, 5) == true)
            {
                Console.WriteLine("Connected to game!");
                Authenticate();
            }
            else
            {
                Console.WriteLine("Failed to connect to game");
            }

            _client.NewUdpMessageReceived += new ClientUdp.NewUdpMessageReceivedEventHandler(OnNewMessageRecieved);
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

        private void OnNewMessageRecieved(object sender, NewUdpServerMessageReceivedEventArgs e)
        {
            MessageReader mr = new MessageReader();
            mr.SetNewMessage(e.Message, 0);

            //AirGameProt protocol = (AirGameProt)mr.ReadByte();

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
