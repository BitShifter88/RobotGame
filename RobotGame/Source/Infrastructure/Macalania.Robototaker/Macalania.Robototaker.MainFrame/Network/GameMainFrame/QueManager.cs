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
        bool _stop = false;
        Thread _queThread;
        GameManager _gameManager;
        NetServer _server;

        public QueManager(GameManager gameManager, NetServer server)
        {
            _server = server;
            _gameManager = gameManager;
        }

        public void AddPlayer(PlayerSession player)
        {
            _playersInQue.Add(player.SessionId, player);
        }

        public void RemovePlayer(PlayerSession player)
        {
            _playersInQue.Remove(player.SessionId);
        }

        public void StartQue()
        {
            _queThread = new Thread(new ThreadStart(QueRun));
            _queThread.Start();
        }

        private void MatchMake()
        {

        }

        private void QueRun()
        {
            while (_stop == false)
            {
                Thread.Sleep(1000);
                MatchMake();
            }
        }

        public void StopQue()
        {
            _stop = true;
        }
    }
}
