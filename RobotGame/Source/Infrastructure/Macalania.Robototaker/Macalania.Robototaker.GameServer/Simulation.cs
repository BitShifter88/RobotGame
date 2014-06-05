using Frame.Network.Server;
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

        public Simulation()
        {
            StartGameLoop();
        }

        protected override void Update(double dt)
        {
            base.Update(dt);
        }

        public void AddPlayer(ClientConnectionUdp connection, string playerName)
        {
            GamePlayer gp = new GamePlayer(connection, playerName);
            Players.Add(connection.Id, gp);
        }
    }
}
