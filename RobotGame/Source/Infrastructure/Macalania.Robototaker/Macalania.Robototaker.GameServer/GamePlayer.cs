using Frame.Network.Server;
using Macalania.Probototaker.Tanks;
using Macalania.YunaEngine.GameLogic;
using Macalania.YunaEngine.Resources;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macalania.Robototaker.GameServer
{
    class GamePlayer : GameObject
    {
        public ClientConnectionUdp Connection { get; private set; }
        public string PlayerName { get; private set; }
        public string SessionId { get; set; }

        Tank _tank;

        public GamePlayer(ClientConnectionUdp connection, string playerName, string sessionId, Room room) : base(room)
        {
            Connection = connection;
            PlayerName = playerName;
            SessionId = sessionId;
        }

        public override void Load(ResourceManager content)
{
 	            _tank = new Tank(Room, new Vector2(0, 0));
            _tank.Inizialize();
            _tank.Load(content);
}
    }
}
