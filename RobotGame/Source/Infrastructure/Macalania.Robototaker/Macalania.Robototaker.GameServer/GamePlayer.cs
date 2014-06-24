using Frame.Network.Common;
using Lidgren.Network;
using Macalania.Probototaker.Network;
using Macalania.Probototaker.Projectiles;
using Macalania.Probototaker.Tanks;
using Macalania.Probototaker.Tanks.Hulls;
using Macalania.Probototaker.Tanks.Plugins;
using Macalania.Probototaker.Tanks.Plugins.MainGuns;
using Macalania.Probototaker.Tanks.Plugins.Mic;
using Macalania.Probototaker.Tanks.Tracks;
using Macalania.Probototaker.Tanks.Turrets;
using Macalania.Robototaker.Log;
using Macalania.Robototaker.Protocol;
using Macalania.YunaEngine.GameLogic;
using Macalania.YunaEngine.Resources;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Macalania.Robototaker.GameServer
{
    class GamePlayer : GameObject
    {
        public NetConnection Connection { get; private set; }
        private NetServer _server;
        public string PlayerName { get; private set; }
        public string SessionId { get; set; }
        public int LastCommandId { get; set; }
        public byte TankId { get; set; }

        public Tank Tank { get; set; }

        Mutex _playerMutex = new Mutex();

        public GamePlayer(NetConnection connection, NetServer server, string playerName, string sessionId, Room room, byte tankId)
            : base(room)
        {
            TankId = tankId;
            Connection = connection;
            PlayerName = playerName;
            SessionId = sessionId;
            _server = server;
        }

        /// <summary>
        /// Sends information to the player about the position and movement of the player parameter
        /// </summary>
        /// <param name="player"></param>
        public void OtherPlayerInfoMovement(GamePlayer player)
        {
            NetOutgoingMessage m = _server.CreateMessage();
            m.Write((byte)RobotProt.OtherPlayerInfoMovement);
            m.Write(player.SessionId);
            m.Write(player.Tank.ServerSideTankId);
            m.Write(player.Tank.Position.X);
            m.Write(player.Tank.Position.Y);
            m.Write(player.Tank.BodyRotation);
            
            m.Write(player.Tank.CurrentSpeed);
            m.Write(player.Tank.CurrentRotationSpeed);

            byte packed = BytePacker.Pack((byte)player.Tank.DrivingDir, (byte)player.Tank.BodyDir, (byte)player.Tank.TurretDir, 0);
            m.Write(packed);

            m.Write(player.Tank.TurretRotation);

            m.Write((ushort)(player.Connection.AverageRoundtripTime*1000f/2f));

            Connection.SendMessage(m, NetDeliveryMethod.Unreliable, 0);
        }

        public void SendAbilityActivation(PluginType type, Vector2 targetPosition, Tank targetTank)
        {

        }

        public void SendProjectileInfo(List<Projectile> projectiles)
        {
            NetOutgoingMessage m = _server.CreateMessage();
            m.Write((byte)RobotProt.ProjectileInfo);

            if (projectiles.Count > 256)
                ServerLog.E("Tried to send more than 256 projectiles. This is not possible", LogType.CriticalError);

            m.Write((byte)projectiles.Count);

            foreach (Projectile p in projectiles)
            {
                m.Write((byte) p.ProjectileType);

                m.Write((byte)p.Source.ServerSideTankId);

                m.Write(p.Position.X);
                m.Write(p.Position.Y);
                m.Write(p.Direction.X);
                m.Write(p.Direction.Y);
            }

            Connection.SendMessage(m, NetDeliveryMethod.Unreliable, 0);
        }

        /// <summary>
        /// Called when the server recieves information about player movement
        /// </summary>
        /// <param name="commandId"></param>
        /// <param name="broadcastCount"></param>
        /// <param name="playerMovement"></param>
        public void PlayerMovement(int commandId, byte broadcastCount, PlayerMovement playerMovement)
        {
            _playerMutex.WaitOne();
            if (commandId != LastCommandId)
            {
                //ServerLog.E("Ping " + (Connection.AverageRoundtripTime * 1000f) / 2f, LogType.Debug);
                int difference = commandId - LastCommandId;
                if (broadcastCount > 0)
                {
                    ServerLog.E("Missing player movement package: " + broadcastCount, LogType.Debug);
                }
                if (difference != 1)
                    ServerLog.E("Arrival rate out of sync of player movement package: " + difference, LogType.Debug);

                //ServerLog.E("When recieve " + Tank.BodyRotation, LogType.GameActivity);

                //double timeinterval = ((double)Connection.Ping) / 100d;

                //for (int i = 0; i < 100; i++)
                //{
                //    Tank.Update(-timeinterval);
                //}
                //ServerLog.E("After reverse " + Tank.BodyRotation, LogType.GameActivity);

                Tank.Thruttle(playerMovement.DrivingDir);
                Tank.RotateBody(playerMovement.BodyDir);
                Tank.TurretDir = playerMovement.TurretDir;
                Tank.TurretRotation = playerMovement.TurretRotation;
                Tank.MainGunFirering = playerMovement.MainGunFirering;

                //Tank.Update(1000d / 60d);

                //for (int i = 0; i < 100; i++)
                //{
                //    Tank.Update(timeinterval);
                //}

                //ServerLog.E("After speed " + Tank.BodyRotation, LogType.GameActivity);

                LastCommandId = commandId;
            }
            else
            {
                ServerLog.E("Double player movement", LogType.Debug);
            }
            _playerMutex.ReleaseMutex();
        }

        public override void Update(double dt)
        {
            base.Update(dt);
        }

        public void BroadcasPositionToPlayer()
        {
            if (Tank == null)
                return;

            Vector2 pos = Tank.MovePosition(Tank.Position, Tank.CurrentSpeed,  1000f / 60f);
            float rot = Tank.DoBodyRotation(Tank.BodyRotation, Tank.CurrentRotationSpeed, 1000f / 60f);

            //Console.WriteLine(Connection.Ping);

            NetOutgoingMessage m = _server.CreateMessage();

            m.Write((byte)RobotProt.PlayerCompensation);
            m.Write(pos.X);
            m.Write(pos.Y);
            m.Write(rot);

            Connection.SendMessage(m, NetDeliveryMethod.Unreliable, 0);
        }

        public override void Load(ResourceManager content)
        {
            Tank t1 = new Tank(Room, new Vector2(100, 600));

            StarterHull sh = new StarterHull();
            sh.SetTank(t1);
            sh.Load(content);
            t1.SetHull(sh);

            StarterTrack st = new StarterTrack();
            st.SetTank(t1);
            st.Load(content);
            t1.SetTrack(st);

            Turret t = new BigTurret();
            t.SetTank(t1);
            t.Load(content);

            t1.SetTurret(t);

            //StarterMainGun smg = new StarterMainGun();
            //smg.Load(content);
            //smg.SetTank(t1);
            //t.AddPluginTop(smg, 1);

            MiniMainGun mg1 = new MiniMainGun();
            mg1.SetTank(t1);
            mg1.Load(content);
            t.AddPluginTop(mg1, 0);

            MiniMainGun mg2 = new MiniMainGun();
            mg2.SetTank(t1);
            mg2.Load(content);
            t.AddPluginTop(mg2, 1);

            MiniMainGun mg3 = new MiniMainGun();
            mg3.SetTank(t1);
            mg3.Load(content);
            t.AddPluginTop(mg3, 2);

            //SprayMainGun smgg = new SprayMainGun(t);
            //smgg.Load(content);
            //smgg.SetTank(t1);
            //t.AddPluginTop(smgg,0);

            //MiniMainGun mg2 = new MiniMainGun();
            //mg2.Load(content);
            //mg2.SetTank(t1);
            //t.AddPluginTop(mg2, 1);

            //AmorPlugin ap1 = new AmorPlugin(PluginDirection.Left);
            //ap1.Load(content);
            //ap1.SetTank(t1);
            //t.AddPluginLeftSide(ap1, 0);

            SunPannelPlugin spp = new SunPannelPlugin(PluginDirection.Left);
            spp.Load(content);
            spp.SetTank(t1);
            t.AddPluginLeftSide(spp, 0);

            //AmorPlugin ap2 = new AmorPlugin(PluginDirection.Left);
            //ap2.Load(content);
            //ap2.SetTank(t1);
            //t.AddPluginLeftSide(ap2, 1);

            BatteryPlugin bt = new BatteryPlugin(PluginDirection.Left);
            bt.Load(content);
            bt.SetTank(t1);
            t.AddPluginLeftSide(bt, 1);

            AmorPlugin ap3 = new AmorPlugin(PluginDirection.Right);
            ap3.Load(content);
            ap3.SetTank(t1);
            t.AddPluginRightSide(ap3, 2);

            ShieldPlugin sp = new ShieldPlugin(PluginDirection.Left);
            sp.Load(content);
            sp.SetTank(t1);
            t.AddPluginLeftSide(sp, 2);

            ArtileryStarter art = new ArtileryStarter();
            art.Load(content);
            art.SetTank(t1);
            t.AddPluginButtom(art, 0);

            StarterAttackRocketBatteryPlugin attack = new StarterAttackRocketBatteryPlugin();
            attack.SetTank(t1);
            attack.Load(content);
            t.AddPluginRightSide(attack, 0);
            //r = new RocketStarterPlugin(PluginDirection.Right);
            //r.SetTank(t1);
            //r.Load(content);
            //t.AddPluginRightSide(r, 0);

            t1.ServerSideTankId = TankId;

            Tank = t1;

            Tank.ReadyTank();

            Room.AddGameObjectWhileRunning(Tank);
        }
    }
}
