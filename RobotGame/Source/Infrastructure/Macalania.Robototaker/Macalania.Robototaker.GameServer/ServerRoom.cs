using Lidgren.Network;
using Macalania.Probototaker.Network;
using Macalania.Probototaker.Projectiles;
using Macalania.Probototaker.Rooms;
using Macalania.Probototaker.ServerGlobals;
using Macalania.Probototaker.Tanks.Plugins;
using Macalania.Robototaker.Log;
using Macalania.YunaEngine.GameLogic;
using Macalania.YunaEngine.Resources;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Macalania.Robototaker.GameServer
{
    class ServerRoom : SimulationRoom
    {
        List<Projectile> _projectilesFiredSinceLast = new List<Projectile>();
        int _updatesSinceProjectileSend = 0;
        public Dictionary<long, GamePlayer> Players { get; private set; }
        List<long> _playersToRemove = new List<long>();
        double _oneSecUpdate = 0;
        Mutex _playerMutex = new Mutex();
        private NetServer _server;


        public ServerRoom(NetServer server)
        {
            _server = server;
            Players = new Dictionary<long, GamePlayer>();
        }

        private void RemovePlayers()
        {
            _playerMutex.WaitOne();
            foreach (long player in _playersToRemove)
            {
                Players[player].RemovedFromServer();
                Players.Remove(player);
            }
            _playersToRemove.Clear();
            _playerMutex.ReleaseMutex();
        }

        public void ActivatePlugin(PluginType type, long connectionId, byte tankTargetId, Vector2 targetPosition)
        {
            if (Players[connectionId].Tank.ActivatePlugin(type, targetPosition, null))
            {

            }
        }

        private void HandleNewProjectiles()
        {
            if (_projectilesFiredSinceLast.Count == 0)
                return;
            if (_updatesSinceProjectileSend == SGlobals.UpdatesBetweenProjectileSends)
            {
                // TODO: PRojektilerne skal vide hvem der affyrede dem
                foreach (GamePlayer p in Players.Values.ToList())
                {
                    p.SendProjectileInfo(_projectilesFiredSinceLast);
                }

                _updatesSinceProjectileSend = 0;
                _projectilesFiredSinceLast.Clear();
            }

            _updatesSinceProjectileSend++;
        }

        private void OneSecUpdate()
        {
            //if (Players.Count > 0)
            //Console.WriteLine(Players.Values.ToList()[0].Connection.Ping);
            List<GamePlayer> players = Players.Values.ToList();
            foreach (GamePlayer player in players)
            {
                player.BroadcasPositionToPlayer();

                //foreach (GamePlayer playerr in Players.Values)
                //{
                //    //if (player.Connection.Id == connectionId)
                //    //    continue;
                //    playerr.OtherPlayerInfoMovement(playerr);
                //}
            }
        }

        public override void Update(double dt)
        {
            RemovePlayers();
            //Console.WriteLine(dt);
            _oneSecUpdate += dt;
            if (_oneSecUpdate >= 500)
            {
                OneSecUpdate();
                _oneSecUpdate = 0;
            }

            _playerMutex.WaitOne();
            base.Update(dt);
            _playerMutex.ReleaseMutex();

            HandleNewProjectiles();
        }

        protected override void RegisterGameObject(GameObject obj)
        {
            if (obj.GetType().IsSubclassOf(typeof(Projectile)))
            {
                Projectile p = (Projectile)obj;
                if (p.Fired)
                    _projectilesFiredSinceLast.Add((Projectile)obj);
            }
            base.RegisterGameObject(obj);
        }

        public void PlayerMovement(long connectionId, int commandId, byte broadcastCount, PlayerMovement playerMovement)
        {
            _playerMutex.WaitOne();
            if (PlayerExists(connectionId) == false)
            {
                _playerMutex.ReleaseMutex();
                return;
            }

            Players[connectionId].PlayerMovement(commandId, broadcastCount, playerMovement);

            foreach (GamePlayer player in Players.Values)
            {
                if (player.Connection.RemoteUniqueIdentifier == connectionId)
                    continue;
                player.OtherPlayerInfoMovement(Players[connectionId]);
            }

            // Broadcast the playerinput imediatly to other players so that they may update their estimates fast (should be removed mabey)
            if (playerMovement.DrivingDir == Probototaker.Tanks.DrivingDirection.Still)
                Players[connectionId].BroadcasPositionToPlayer();

            _playerMutex.ReleaseMutex();
        }

        private bool PlayerExists(long connectionId)
        {
            if (Players.ContainsKey(connectionId))
                return true;
            else
            {
                ServerLog.E("Tried to do something with a non existing player", LogType.PossibleBug);
                return false;
            }
        }

        public void DisconnectedPlayer(NetConnection connection)
        {
            _playerMutex.WaitOne();
            foreach (KeyValuePair<long, GamePlayer> gp in Players)
            {
                if (gp.Value.Connection.RemoteUniqueIdentifier == connection.RemoteUniqueIdentifier)
                    _playersToRemove.Add(gp.Key);
            }
            _playerMutex.ReleaseMutex();
        }

        public void AddPlayer(NetConnection connection, string playerName, string sessionId, byte tankId)
        {

            _playerMutex.WaitOne();
            GamePlayer gp = new GamePlayer(connection, _server, playerName, sessionId, this, tankId);
            
            Players.Add(connection.RemoteUniqueIdentifier, gp);
            
            AddGameObjectWhileRunning(gp);
            ServerLog.E("Player added: " + sessionId, LogType.GameActivity);
            
            _playerMutex.ReleaseMutex();

        }
    }
}
