using Lidgren.Network;
using Macalania.Robototaker.MainFrame.Network.ServerMainFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Robototaker.MainFrame.Network.GameMainFrame
{
    class GameManager
    {
        Dictionary<short, GameInstance> _games = new Dictionary<short, GameInstance>();
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

        public void PlayerIsReady(PlayerSession player, short gameId)
        {
            _games[gameId].PlayerIsReady(player);
        }

        public void CheckGames()
        {
            foreach (KeyValuePair<short, GameInstance> g in _games)
            {
                g.Value.Update();

                if (g.Value.Status == GameInstanceStatus.PlayersReady)
                {
                    WorkScheduler.Instance.RequestServer(g.Key);
                    g.Value.Creating();
                }
                if (g.Value.Status == GameInstanceStatus.CreatingGame)
                {
                    ServerRequest sr = WorkScheduler.Instance.GetServerRequest(g.Key);

                    if (sr.Status == ServerRequestStatus.ServerHosted)
                    {
                        g.Value.Running(sr.ServerIp);
                    }
                }

            }
        }
    }
}
