using Macalania.Probototaker.Effects;
using Macalania.Probototaker.Network;
using Macalania.Probototaker.Tanks;
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

        public GameRoom(GameNetwork gn)
        {
            _gn = gn;
            OtherPlayers = new Dictionary<byte, OtherPlayer>();
        }

        public override void Inizialize()
        {
            Player = new Player(this);
            AddGameObject(Player);

            OtherPlayer op = new OtherPlayer(this, 1);
            AddGameObject(op);

            OtherPlayer op2 = new OtherPlayer(this, 2);
            AddGameObject(op2);

            OtherPlayer op3 = new OtherPlayer(this, 3);
            AddGameObject(op3);

            GhostPlayer = new OtherPlayer(this, 1);
            AddGameObject(GhostPlayer);

            base.Inizialize();
        }

        public void OtherPlayerInfoMovement(string sessionId, Vector2 position, float bodyRotation, float bodySpeed, float rotationSpeed, DrivingDirection drivingDir, RotationDirection bodyDir, RotationDirection turretDir, float turretRotation, ushort ping, byte tankId)
        {
            if (OtherPlayers.ContainsKey(tankId) == false)
            {
                OtherPlayer op = new OtherPlayer(this, 1);
                this.AddGameObjectWhileRunning(op);

                // TODO: Alt det her skal flyttes ind i other player klassen
                op.SetPosition(position);
                op.GetTank().BodyRotation = bodyRotation;
                op.GetTank().DrivingDir = drivingDir;
                op.GetTank().BodyDir = bodyDir;
                op.GetTank().CurrentSpeed = bodySpeed;
                op.GetTank().BodyRotation = rotationSpeed;
                op.GetTank().TurretDir = turretDir;
                op.GetTank().TurretRotation = turretRotation;
                op.GetTank().ServerSideTankId = tankId;

                // The info from the srver is old. We fastforward the time for the tank, assuming drivingDir and rotationDir has not changed.
                // The tank is aproxematly now the same place as on the other client
                int totalDelay = (int)((float)ping + (_gn.GetClientUdp().Connections[0].AverageRoundtripTime * 1000f / 2f));
                int updatesBehind = (int)((double)totalDelay / (1000d / 60d));

                Console.WriteLine(updatesBehind);

                for (int i = 0; i < updatesBehind; i++)
                {
                    op.GetTank().Update(1000d / 60d);
                }

                op.GetTank().SetServerEstimation(op.GetTank().Position, op.GetTank().BodyRotation, op.GetTank().CurrentSpeed, op.GetTank().CurrentRotationSpeed, turretRotation);

                OtherPlayers.Add(tankId, op);
            }

            OtherPlayers[tankId].PlayerInfoMovement(position, bodyRotation, bodySpeed, rotationSpeed, drivingDir, bodyDir, turretDir, turretRotation, ping, (int)(_gn.GetClientUdp().Connections[0].AverageRoundtripTime * 1000f / 2f));
        }

        private void FirstRun()
        {

            Camera = new TankCamera(Player.GetTank());

        }

        public override void Update(double dt)
        {
            if (_firstRun)
            {
                FirstRun();
                _firstRun = false;
            }
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
