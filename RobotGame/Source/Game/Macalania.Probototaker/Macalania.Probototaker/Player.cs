using Macalania.Probototaker.Network;
using Macalania.Probototaker.Rooms;
using Macalania.Probototaker.Tanks;
using Macalania.Probototaker.Tanks.Hulls;
using Macalania.Probototaker.Tanks.Plugins;
using Macalania.Probototaker.Tanks.Plugins.MainGuns;
using Macalania.Probototaker.Tanks.Plugins.Mic;
using Macalania.Probototaker.Tanks.Tracks;
using Macalania.Probototaker.Tanks.Turrets;
using Macalania.YunaEngine;
using Macalania.YunaEngine.GameLogic;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Input;
using Macalania.YunaEngine.Rendering;
using Macalania.YunaEngine.Resources;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker
{
    public class Player : GameObject
    {
        Tank _tank;
        ShieldPlugin sp;
        RocketStarterPlugin r;
        ArtileryStarter art;
        MineLayerPlugin mlp;
        StarterAttackRocketBatteryPlugin attack;
        GameRoom _gameRoom;

        public Player(Room room)
            : base(room)
        {
            _gameRoom = (GameRoom)room;
        }
        public override void Inizialize()
        {
            base.Inizialize();
        }

        public Tank GetTank()
        {
            return _tank;
        }
        public override void Load(ResourceManager content)
        {
            base.Load(content);

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

            sp = new ShieldPlugin(PluginDirection.Left);
            sp.Load(content);
            sp.SetTank(t1);
            t.AddPluginLeftSide(sp, 2);

            //art = new ArtileryStarter();
            //art.Load(content);
            //art.SetTank(t1);
            //t.AddPluginButtom(art, 0);

            mlp = new MineLayerPlugin();
            mlp.Load(content);
            mlp.SetTank(t1);
            t.AddPluginButtom(mlp, 0);

            attack = new StarterAttackRocketBatteryPlugin();
            attack.SetTank(t1);
            attack.Load(content);
            t.AddPluginRightSide(attack, 0);
            //r = new RocketStarterPlugin(PluginDirection.Right);
            //r.SetTank(t1);
            //r.Load(content);
            //t.AddPluginRightSide(r, 0);

            _tank = t1;

            _tank.ReadyTank();

            Room.AddGameObjectWhileRunning(_tank);
        }
        private void HandleInput()
        {
            if (KeyboardInput.IsKeyUp(Keys.A) && KeyboardInput.IsKeyUp(Keys.D))
                _tank.RotateBody(RotationDirection.Still);
            if (KeyboardInput.IsKeyDown(Keys.A))
            {
                _tank.RotateBody(RotationDirection.CounterClockWise);
            }
            if (KeyboardInput.IsKeyDown(Keys.D))
            {
                _tank.RotateBody(RotationDirection.ClockWise);
            }
            if (KeyboardInput.IsKeyUp(Keys.W) && KeyboardInput.IsKeyUp(Keys.S))
                _tank.Thruttle(DrivingDirection.Still);
            if (KeyboardInput.IsKeyDown(Keys.W))
                _tank.Thruttle(DrivingDirection.Forward);
            if (KeyboardInput.IsKeyDown(Keys.S))
                _tank.Thruttle(DrivingDirection.Backwards);

            if (MouseInput.IsLeftMousePressed())
            {
                _tank.FireMainGun();
            }

            if (KeyboardInput.IsKeyClicked(Keys.NumPad1))
                _tank.ActivatePlugin(sp, Vector2.Zero, null);
            if (KeyboardInput.IsKeyClicked(Keys.NumPad2))
                _tank.ActivatePlugin(art, new Vector2(MouseInput.X, MouseInput.Y), null);
            if (KeyboardInput.IsKeyClicked(Keys.NumPad3))
            {
                _tank.ActivatePlugin(r, Vector2.Zero, null);
            }
            if (KeyboardInput.IsKeyClicked(Keys.NumPad4))
                _tank.ActivatePlugin(attack, Vector2.Zero, null);
            if (KeyboardInput.IsKeyClicked(Keys.NumPad5))
                _tank.ActivatePlugin(mlp, Vector2.Zero, null);

            //_tank.MoveTurretTowardsPoint(new Vector2(MouseInput.X, MouseInput.Y));

            _gameRoom.GameCommunication.PlayerMovement(new PlayerMovement() { DrivingDir = _tank.DrivingDir, RotationDir = _tank.RotationDir });
        }

        public void PlayerCompensation(Vector2 position, float bodyRotation, int latency)
        {
            _tank.SetLastKnownServerInfo(position, bodyRotation, latency);
        }
        
        public override void Update(double dt)
        {
            base.Update(dt);
            YunaGameEngine.Instance.Window.Title = _tank.BodyRotation.ToString();
            HandleInput();
        }
        public override void Draw(IRender render, Camera camera)
        {
            base.Draw(render, camera);

        }
    }
}
