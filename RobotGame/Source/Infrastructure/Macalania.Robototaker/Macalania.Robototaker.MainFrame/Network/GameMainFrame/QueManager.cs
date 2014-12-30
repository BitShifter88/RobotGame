using Lidgren.Network;
using Macalania.Robototaker.Log;
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

        int _playersInAGame = 1;

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

            if (_playersInQue.Count > _playersInAGame-1)
            {
                CreateGame();
            }

            _queMutex.ReleaseMutex();
        }

        private void CreateGame()
        {
            ServerLog.E("Game created", LogType.Information);
            List<PlayerSession> players;
            players = _playersInQue.Values.ToList().Take(_playersInAGame).ToList();
            _playersInQue.Clear();

            _gameManager.CreateNewGame(players);
        }
    }
}
