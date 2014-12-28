using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Macalania.Robototaker.MainFrame.Network.GameMainFrame
{
    class QueManager
    {
        Dictionary<int, PlayerSession> _playersInQue = new Dictionary<int, PlayerSession>();

        GameManager _gameManager;
        NetServer _server;
        Mutex _queMutex = new Mutex();

        public QueManager(GameManager gameManager, NetServer server)
        {
            _server = server;
            _gameManager = gameManager;
        }

        public void AddPlayer(PlayerSession player)
        {
            _queMutex.WaitOne();
            _playersInQue.Add(player.SessionId, player);
            _queMutex.ReleaseMutex();
        }

        public void RemovePlayer(PlayerSession player)
        {
            _queMutex.WaitOne();
            _playersInQue.Remove(player.SessionId);
            _queMutex.ReleaseMutex();
        }

        public void MatchMake()
        {
            _queMutex.WaitOne();

            if (_playersInQue.Count > 0)
            {
                List<PlayerSession> players;
                players = _playersInQue.Values.ToList().Take(1).ToList();
                _playersInQue.Remove(players[0].SessionId);

                _gameManager.CreateNewGame(players);
            }


            _queMutex.ReleaseMutex();
        }
    }
}
