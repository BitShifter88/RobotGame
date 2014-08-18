using Macalania.Probototaker.Network;
using Macalania.Probototaker.Rooms;
using Macalania.Probototaker.Tanks;
using Macalania.Probototaker.Tanks.Hulls;
using Macalania.Probototaker.Tanks.Turrets;
using Macalania.Probototaker.Tanks.Plugins;
using Macalania.Probototaker.Tanks.Plugins.Mic;
using Macalania.Probototaker.Tanks.Tracks;
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
using Macalania.Probototaker.Tanks.Plugins.MainGuns;

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
        TankPackage _tp;

        public Player(Room room, TankPackage tp)
            : base(room)
        {
            _tp = tp;
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

            Tank t1 = Tank.GetTankFromPackage(new Vector2(1000, 600), _tp, content, Room);

            _tank = t1;

            _tank.ReadyTank(Room);


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
            else
                _tank.StopFireMainGun();

            if (KeyboardInput.IsKeyClicked(Keys.NumPad1))
                //_tank.ActivatePlugin(sp, Vector2.Zero, null);
                _gameRoom.GameCommunication.SendAbilityActivation(PluginType.Shield, null, Vector2.Zero);
            if (KeyboardInput.IsKeyClicked(Keys.NumPad2))
                //_tank.ActivatePlugin(PluginType.ArtileryStart, Room.Camera.ProjectPositionWithZoom(MouseInput.X, MouseInput.Y), null, new Random());
                _gameRoom.GameCommunication.SendAbilityActivation(PluginType.ArtileryStart, null, new Vector2(MouseInput.X, MouseInput.Y));
            if (KeyboardInput.IsKeyClicked(Keys.NumPad3))
            {
                _gameRoom.GameCommunication.SendAbilityActivation(PluginType.RocketStarter, null, Vector2.Zero);
                //_tank.ActivatePlugin(PluginType.RocketStarter, Vector2.Zero, null);
            }
            //if (KeyboardInput.IsKeyClicked(Keys.NumPad4))
            //    _tank.ActivatePlugin(attack, Vector2.Zero, null);
            //if (KeyboardInput.IsKeyClicked(Keys.NumPad5))
            //    _tank.ActivatePlugin(mlp, Vector2.Zero, null);

            _tank.MoveTurretTowardsPoint(Room.Camera.ProjectPosition(MouseInput.X, MouseInput.Y));

            _gameRoom.GameCommunication.PlayerMovement(new PlayerMovement() { DrivingDir = _tank.DrivingDir, BodyDir = _tank.BodyDir, TurretDir = _tank.TurretDir, TurretRotation = _tank.TurretRotation, MainGunFirering = _tank.MainGunFirering });
        }

        public void PlayerCompensation(Vector2 position, float bodyRotation, int latency)
        {
            _tank.SetLastKnownServerInfo(position, bodyRotation, latency);
        }
        
        public override void Update(double dt)
        {
            base.Update(dt);
            YunaGameEngine.Instance.Window.Title = _tank.Position.ToString();
            HandleInput();
        }
        public override void Draw(IRender render, Camera camera)
        {
            base.Draw(render, camera);

        }
    }
}
