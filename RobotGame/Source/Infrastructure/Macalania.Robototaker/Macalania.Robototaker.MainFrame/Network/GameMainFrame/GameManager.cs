using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Robototaker.MainFrame.Network.GameMainFrame
{
    class GameManager
    {
        Dictionary<int, GameInstance> _games = new Dictionary<int, GameInstance>();
        NetServer _server;

        public GameManager(NetServer server)
        {
            _server = server;
        }

        public void CreateNewGame(List<PlayerSession> players)
        {
            GameInstance g = new GameInstance(_server);

            foreach (PlayerSession p in players)
            {
                g.AddPlayer(p);
            }

            _games.Add(g.GameId, g);

            g.AskPlayersIfReady();
        }

        public void PlayerIsReady(PlayerSession player, int gameId)
        {
            _games[gameId].PlayerIsReady(player);
        }

        public void CheckGames()
        {
            foreach (KeyValuePair<int, GameInstance> g in _games)
            {
                g.Value.UpdateStatus();
            }
        }
    }
}
