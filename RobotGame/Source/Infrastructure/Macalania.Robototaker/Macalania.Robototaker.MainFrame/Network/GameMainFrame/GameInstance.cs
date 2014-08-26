using Lidgren.Network;
using Macalania.Robototaker.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Robototaker.MainFrame.Network.GameMainFrame
{
    enum GameInstanceStatus
    {
        PlayersNotReady,
        PlayersReady,
        Running,
        SuccessFinished,
        Aborted, // The game was running, but then got aborted (i.e the server crashed)
        Cancled, // The game never started (i.e. one of the players disconnected before it could start)
    }

    class GameInstance
    {
        public int GameId { get; set; }
        public GameInstanceStatus Status { get; set; }

        List<PlayerSession> _players = new List<PlayerSession>();
        List<PlayerSession> _readyPlayers = new List<PlayerSession>();
        GServer _server;
        int _askIfReadyTimeout = 5000;
        
        public GameInstance(GServer server)
        {
            GameId = GameInstanceIdGenerator.GetNextId();
            _server = server;
            Status = GameInstanceStatus.PlayersNotReady;
        }

        public void AddPlayer(PlayerSession player)
        {
            _players.Add(player);
        }

        public void AskPlayersIfReady()
        {
            foreach (PlayerSession p in _players)
            {
                NetOutgoingMessage m = _server.GetServer().CreateMessage();
                m.Write((byte)MainFrameProt.AskIfReadyForGame);
                m.Write(GameId);
                p.Connection.SendMessage(m, NetDeliveryMethod.ReliableUnordered, 0);
            }
        }

        public void PlayerIsReady(PlayerSession player)
        {
            if (_readyPlayers.Contains(player) == false)
                _readyPlayers.Add(player);
        }
    }
}
