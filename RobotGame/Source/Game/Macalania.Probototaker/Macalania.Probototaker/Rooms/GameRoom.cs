using Macalania.Probototaker.Effects;
using Macalania.Probototaker.Map;
using Macalania.Probototaker.Network;
using Macalania.Probototaker.Tanks;
using Macalania.Probototaker.Tanks.Plugins;
using Macalania.YunaEngine;
using Macalania.YunaEngine.GameLogic;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Macalania.Probototaker.Rooms
{
    public class GameRoom : SimulationRoom
    {
        public GameCommunication GameCommunication { get; private set; }
        public Dictionary<byte, OtherPlayer> OtherPlayers { get; set; }
        public OtherPlayer GhostPlayer { get; set; }
        public Player Player { get; set; }
        GameNetwork _gn;
        bool _firstRun = true;
        TankPackage _tp;
        public GameMap GameMap { get; set; }
        Mutex _gameMutex = new Mutex();

        public GameRoom(GameNetwork gn, TankPackage tp)
        {
            _tp = tp;
            _gn = gn;
            GameCommunication = new GameCommunication(_gn);
            OtherPlayers = new Dictionary<byte, OtherPlayer>();
        }

        public override void Inizialize()
        {
            Player = new Player(this, _tp);
            AddGameObject(Player);

            OtherPlayer op = new OtherPlayer(this, 1, null);
            AddGameObject(op);

            OtherPlayer op2 = new OtherPlayer(this, 2, null);
            AddGameObject(op2);

            OtherPlayer op3 = new OtherPlayer(this, 3, null);
            AddGameObject(op3);

            GhostPlayer = new OtherPlayer(this, 1, null);
            AddGameObject(GhostPlayer);

            base.Inizialize();
        }

        public override void Load(IServiceProvider serviceProvider)
        {
            base.Load(serviceProvider);

            GameMap = GameMap.LoadFromFile("Maps/map.ptm", this);
            AddGameObject(GameMap);
            
        }

        public void CreateOtherPlayer(byte tankId, Vector2 position, float bodyRotation, float bodySpeed, float rotationSpeed, DrivingDirection drivingDir, RotationDirection bodyDir, RotationDirection turretDir, float turretRotation, TankPackage tp, ushort ping)
        {
            _gameMutex.WaitOne();
            OtherPlayer op = new OtherPlayer(this, 0, tp);
            this.AddGameObjectWhileRunning(op);

            op.SetPosition(position);
            op.GetTank().BodyRotation = bodyRotation;
            op.GetTank().DrivingDir = drivingDir;
            op.GetTank().BodyDir = bodyDir;
            op.GetTank().CurrentSpeed = bodySpeed;
            op.GetTank().BodyRotation = rotationSpeed;
            op.GetTank().TurretDir = turretDir;
            op.GetTank().TurretRotation = turretRotation;
            op.GetTank().ServerSideTankId = tankId;

            int totalDelay = (int)((float)ping + (_gn.GetClientUdp().Connections[0].AverageRoundtripTime * 1000f / 2f));
            int updatesBehind = (int)((double)totalDelay / (1000d / 60d));

            //Console.WriteLine(updatesBehind);

            for (int i = 0; i < updatesBehind; i++)
            {
                op.GetTank().Update(1000d / 60d);
            }

            op.GetTank().SetServerEstimation(op.GetTank().Position, op.GetTank().BodyRotation, op.GetTank().CurrentSpeed, op.GetTank().CurrentRotationSpeed, turretRotation);

            op.PlayerInfoMovement(position, bodyRotation, bodySpeed, rotationSpeed, drivingDir, bodyDir, turretDir, turretRotation, ping, (int)(_gn.GetClientUdp().Connections[0].AverageRoundtripTime * 1000f / 2f));

            OtherPlayers.Add(tankId, op);

            _gameMutex.ReleaseMutex();
        }

        public void PlayerUsesAbility(byte tankId, PluginType type, byte targetTank, Vector2 targetPosition)
        {

        }

        public void OtherPlayerInfoMovement(string sessionId, Vector2 position, float bodyRotation, float bodySpeed, float rotationSpeed, DrivingDirection drivingDir, RotationDirection bodyDir, RotationDirection turretDir, float turretRotation, ushort ping, byte tankId)
        {
            if (OtherPlayers.ContainsKey(tankId) == false)
            {
                Console.WriteLine("Discarded player movement from unknown player");
                return;
                //OtherPlayer op = new OtherPlayer(this, 1, null);
                //this.AddGameObjectWhileRunning(op);

                //// TODO: Alt det her skal flyttes ind i other player klassen
                //op.SetPosition(position);
                //op.GetTank().BodyRotation = bodyRotation;
                //op.GetTank().DrivingDir = drivingDir;
                //op.GetTank().BodyDir = bodyDir;
                //op.GetTank().CurrentSpeed = bodySpeed;
                //op.GetTank().BodyRotation = rotationSpeed;
                //op.GetTank().TurretDir = turretDir;
                //op.GetTank().TurretRotation = turretRotation;
                //op.GetTank().ServerSideTankId = tankId;

                //// The info from the srver is old. We fastforward the time for the tank, assuming drivingDir and rotationDir has not changed.
                //// The tank is aproxematly now the same place as on the other client
                //int totalDelay = (int)((float)ping + (_gn.GetClientUdp().Connections[0].AverageRoundtripTime * 1000f / 2f));
                //int updatesBehind = (int)((double)totalDelay / (1000d / 60d));

                //Console.WriteLine(updatesBehind);

                //for (int i = 0; i < updatesBehind; i++)
                //{
                //    op.GetTank().Update(1000d / 60d);
                //}

                //op.GetTank().SetServerEstimation(op.GetTank().Position, op.GetTank().BodyRotation, op.GetTank().CurrentSpeed, op.GetTank().CurrentRotationSpeed, turretRotation);

                //OtherPlayers.Add(tankId, op);
            }
            
            OtherPlayers[tankId].PlayerInfoMovement(position, bodyRotation, bodySpeed, rotationSpeed, drivingDir, bodyDir, turretDir, turretRotation, ping, (int)(_gn.GetClientUdp().Connections[0].AverageRoundtripTime * 1000f / 2f));
        }

        private void FirstRun()
        {

            Camera = new TankCamera(Player.GetTank());

        }

        public override void Update(double dt)
        {
            _gn.ReadMessages();
            if (_firstRun)
            {
                FirstRun();
                _firstRun = false;
            }
            //Console.WriteLine(dt);
            _gameMutex.WaitOne();
            base.Update(dt);
            _gameMutex.ReleaseMutex();
            _gn.ReadMessages();
        }

        public void ReadyGameCommunication()
        {

            GameCommunication.ReadyGameCommunication();
        }

        public void PlayerCompensation(Vector2 position, float bodyRotation, int latency)
        {
            //GhostPlayer.PlayerGameInfo(position, bodyRotation, latency, Player);
            Player.PlayerCompensation(position, bodyRotation, latency);
        }
    }
}
