using Frame.Network.Client;
using Frame.Network.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Network
{
    class GameNetwork
    {
        ClientUdp _client;

        public void Start()
        {
            SetupServerConnection();
        }

        private void SetupServerConnection()
        {
            _client = new ClientUdp();

            Console.WriteLine("Trying to connect to game...");
            if (_client.Connect("127.0.0.1", 9999, 5) == true)
            {
                Console.WriteLine("Connected to game!");
            }
            else
            {
                Console.WriteLine("Failed to connect to game");
            }

            _client.NewUdpMessageReceived += new ClientUdp.NewUdpMessageReceivedEventHandler(OnNewMessageRecieved);
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
