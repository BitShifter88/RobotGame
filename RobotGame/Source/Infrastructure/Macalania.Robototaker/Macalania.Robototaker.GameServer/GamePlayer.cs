using Frame.Network.Server;
using Macalania.Probototaker.Tanks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macalania.Robototaker.GameServer
{
    class GamePlayer
    {
        public ClientConnectionUdp Connection { get; private set; }
        public string PlayerName { get; private set; }

        Tank _tank;

        public GamePlayer(ClientConnectionUdp connection, string playerName)
        {
            Connection = connection;
            PlayerName = playerName;
        }
    }
}
