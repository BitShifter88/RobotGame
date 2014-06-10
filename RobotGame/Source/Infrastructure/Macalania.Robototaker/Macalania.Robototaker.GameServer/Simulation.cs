using Frame.Network.Server;
using Macalania.Robototaker.Log;
using Macalania.YunaEngine.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macalania.Robototaker.GameServer
{
    class Simulation : GameLoop
    {
        public Dictionary<long, GamePlayer> Players { get; private set; }
        ResourceManager _content;
        ServerRoom _room;

        public Simulation(ResourceManager content)
        {
            _content = content;
            _room = new ServerRoom();
            _room.Load(content);
            Players = new Dictionary<long, GamePlayer>();
            StartGameLoop();
        }

        protected override void Update(double dt)
        {
            _room.Update(dt);
            base.Update(dt);
        }

        public void AddPlayer(ClientConnectionUdp connection, string playerName, string sessionId)
        {
            GamePlayer gp = new GamePlayer(connection, playerName, sessionId, _room);
            Players.Add(connection.Id, gp);
            _room.AddGameObjectWhileRunning(gp);
            ServerLog.E("Player added: " + sessionId, LogType.GameActivity);
        }
    }
}
