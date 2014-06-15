using Frame.Network.Server;
using Macalania.Probototaker.Network;
using Macalania.Robototaker.Log;
using Macalania.YunaEngine.Resources;
using Macalania.YunaEngine.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Macalania.Robototaker.GameServer
{
    class Simulation : GameLoop
    {
        public Dictionary<int, GamePlayer> Players { get; private set; }
        List<int> _playersToRemove = new List<int>();
        ResourceManager _content;
        ServerRoom _room;
        double _oneSecUpdate = 0;
        Mutex _playerMutex = new Mutex();

        public Simulation(ResourceManager content)
        {
            _content = content;
            RoomManager rm = new RoomManager();
            
            _room = new ServerRoom();
            rm.SetActiveRoom(_room, true, content);
            Players = new Dictionary<int, GamePlayer>();
            StartGameLoop();
        }

        private void OneSecUpdate()
        {
            if (Players.Count > 0)
            Console.WriteLine(Players.Values.ToList()[0].Connection.Ping);
            List<GamePlayer> players = Players.Values.ToList();
            foreach (GamePlayer player in players)
            {
                player.BroadcasPositionToPlayer();
            }
        }

        private void RemovePlayers()
        {
            _playerMutex.WaitOne();
            foreach (int player in _playersToRemove)
            {
                Players.Remove(player);
            }
            _playersToRemove.Clear();
            _playerMutex.ReleaseMutex();
        }

        protected override void Update(double dt)
        {
            RemovePlayers();
            Console.WriteLine(dt);
            _oneSecUpdate += dt;
            if (_oneSecUpdate >= 500)
            {
                OneSecUpdate();
                _oneSecUpdate = 0;
            }
            _room.Update(dt);

            base.Update(dt);
        }

        public void PlayerMovement(int connectionId, int commandId, byte broadcastCount, PlayerMovement playerMovement)
        {
            _playerMutex.WaitOne();
            if (PlayerExists(connectionId) == false)
            {
                _playerMutex.ReleaseMutex();
                return;
            }
            
            Players[connectionId].PlayerMovement(commandId, broadcastCount, playerMovement);
            _playerMutex.ReleaseMutex();
        }

        private bool PlayerExists(int connectionId)
        {
            if (Players.ContainsKey(connectionId))
                return true;
            else
            {
                ServerLog.E("Tried to do something with a non existing player", LogType.PossibleBug);
                return false;
            }
        }



        public void DisconnectedPlayer(ClientConnectionUdp connection)
        {
            _playerMutex.WaitOne();
            foreach (KeyValuePair<int, GamePlayer> gp in Players)
            {
                if (gp.Value.Connection.Id == connection.Id)
                    _playersToRemove.Add(gp.Key);
            }
            _playerMutex.ReleaseMutex();
        }

        public void AddPlayer(ClientConnectionUdp connection, string playerName, string sessionId)
        {
            _playerMutex.WaitOne();
            GamePlayer gp = new GamePlayer(connection, playerName, sessionId, _room);
            Players.Add(connection.Id, gp);
            _room.AddGameObjectWhileRunning(gp);
            ServerLog.E("Player added: " + sessionId, LogType.GameActivity);
            _playerMutex.ReleaseMutex();
        }
    }
}
