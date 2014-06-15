using Macalania.Probototaker.Effects;
using Macalania.Probototaker.Network;
using Macalania.Probototaker.Tanks;
using Macalania.YunaEngine;
using Macalania.YunaEngine.GameLogic;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Macalania.Probototaker.Rooms
{
    public class GameRoom : SimulationRoom
    {
        public GameCommunication GameCommunication { get; set; }
        public Dictionary<string, OtherPlayer> OtherPlayers { get; set; }
        public OtherPlayer GhostPlayer { get; set; }
        public Player Player { get; set; }
        GameNetwork _gn;

        public GameRoom(GameNetwork gn)
        {
            _gn = gn;
            OtherPlayers = new Dictionary<string, OtherPlayer>();
        }

        public override void Inizialize()
        {
            Player = new Player(this);
            AddGameObject(Player);

            OtherPlayer op = new OtherPlayer(this, 1);
            AddGameObject(op);

            OtherPlayer op2 = new OtherPlayer(this, 2);
            AddGameObject(op2);

            GhostPlayer = new OtherPlayer(this, 1);
            AddGameObject(GhostPlayer);

            base.Inizialize();
        }

        public override void Update(double dt)
        {
            //Console.WriteLine(dt);
            base.Update(dt);
        }

        public void ReadyGameCommunication()
        {
            GameCommunication = new GameCommunication(_gn);

            GameCommunication.ReadyGameCommunication();
        }

        public void PlayerCompensation(Vector2 position, float bodyRotation, int latency)
        {
            //GhostPlayer.PlayerGameInfo(position, bodyRotation, latency, Player);
            Player.PlayerCompensation(position, bodyRotation, latency);
        }
    }
}
