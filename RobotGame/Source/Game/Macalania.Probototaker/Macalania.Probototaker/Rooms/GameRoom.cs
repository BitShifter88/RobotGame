using Macalania.Probototaker.Effects;
using Macalania.Probototaker.Network;
using Macalania.Probototaker.Tanks;
using Macalania.YunaEngine;
using Macalania.YunaEngine.GameLogic;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
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
        public Dictionary<string, OtherPlayer> OtherPlayers { get; set; }
        public OtherPlayer GhostPlayer { get; set; }
        public Player Player { get; set; }
        GameNetwork _gn;

        public GameRoom(GameNetwork gn)
        {
            _gn = gn;
            OtherPlayers = new Dictionary<string, OtherPlayer>();
        }

        public override void Inizialize()
        {
            Player = new Player(this);
            AddGameObject(Player);

            OtherPlayer op = new OtherPlayer(this, 1);
            AddGameObject(op);

            OtherPlayer op2 = new OtherPlayer(this, 2);
            AddGameObject(op2);

            GhostPlayer = new OtherPlayer(this, 1);
            AddGameObject(GhostPlayer);

            base.Inizialize();
        }

        public void OtherPlayerInfoMovement(string sessionId, Vector2 position, float bodyRotation, float bodySpeed, float rotationSpeed, DrivingDirection drivingDir, RotationDirection rotationDir, ushort ping)
        {
            if (OtherPlayers.ContainsKey(sessionId) == false)
            {
                OtherPlayer op = new OtherPlayer(this, 1);
                this.AddGameObjectWhileRunning(op);

                // TODO: Alt det her skal flyttes ind i other player klassen
                op.SetPosition(position);
                op.GetTank().BodyRotation = bodyRotation;
                op.GetTank().DrivingDir = drivingDir;
                op.GetTank().RotationDir = rotationDir;
                op.GetTank().CurrentSpeed = bodySpeed;
                op.GetTank().BodyRotation = rotationSpeed;

                // The info from the srver is old. We fastforward the time for the tank, assuming drivingDir and rotationDir has not changed.
                // The tank is aproxematly now the same place as on the other client
                int totalDelay = ping + _gn.GetClientUdp().Connection.Ping;
                int updatesBehind = (int)((double)totalDelay / (1000d / 60d)) + 1;

                for (int i = 0; i < updatesBehind; i++)
                {
                    op.GetTank().Update(1000d / 60d);
                }

                op.GetTank().SetServerEstimation(op.GetTank().Position, op.GetTank().BodyRotation, op.GetTank().CurrentSpeed, op.GetTank().CurrentRotationSpeed);

                OtherPlayers.Add(sessionId, op);
            }

            OtherPlayers[sessionId].PlayerInfoMovement(position, bodyRotation, bodySpeed, rotationSpeed, drivingDir, rotationDir, ping, _gn.GetClientUdp().Connection.Ping);
        }

        public override void Update(double dt)
        {
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
