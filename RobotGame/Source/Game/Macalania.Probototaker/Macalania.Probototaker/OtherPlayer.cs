using Macalania.Probototaker.Tanks;
using Macalania.Probototaker.Tanks.Hulls;
using Macalania.Probototaker.Tanks.Plugins;
using Macalania.Probototaker.Tanks.Plugins.MainGuns;
using Macalania.Probototaker.Tanks.Plugins.Mic;
using Macalania.Probototaker.Tanks.Tracks;
using Macalania.Probototaker.Tanks.Turrets;
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
using System.Threading;

namespace Macalania.Probototaker
{
    public class OtherPlayer : GameObject
    {
        Tank _tank;
        int _tankNumber;

        public OtherPlayer(Room room, int tankNumber)
            : base(room)
        {
            _tankNumber = tankNumber;
        }
        public override void Inizialize()
        {
            base.Inizialize();
        }

        public Tank GetTank()
        {
            return _tank;
        }

        public override void SetPosition(Vector2 position)
        {
            _tank.SetPosition(position);
            base.SetPosition(position);
        }

        public override void Load(ResourceManager content)
        {
            if (_tankNumber == 1)
                _tank = TankGenerator.GenerateTank3(Room, content, new Vector2(800, 200));
            if (_tankNumber == 2)
                _tank = TankGenerator.GenerateTank2(Room, content, new Vector2(1000, 200));
            if (_tankNumber == 3)
                _tank = TankGenerator.GenerateTank1(Room, content, new Vector2(600, 200));

            Room.AddGameObjectWhileRunning(_tank);

            base.Load(content);
        }

        public void PlayerInfoMovement(Vector2 position, float bodyRotation, float bodySpeed, float rotationSpeed, DrivingDirection drivingDir, RotationDirection rotationDir, RotationDirection turretDir, float turretRotation, ushort otherClientPing, int playerPing)
        {
           // Console.WriteLine(drivingDir);

            _tank.DrivingDir = drivingDir;
            _tank.BodyDir = rotationDir;
            _tank.TurretDir = turretDir;

            _tank.SetServerEstimation(position, bodyRotation, bodySpeed, rotationSpeed, turretRotation);

            int totalDelay = otherClientPing + playerPing;
            int updatesBehind = (int)((double)totalDelay / (1000d / 60d)) + 1;

            //Console.WriteLine(updatesBehind);

            // Skruer tiden for meget frem af, for some reason
            for (int i = 0; i < updatesBehind; i++)
            {
                _tank.UpdateServerEstimation(1000d / 60d);
            }
            Console.WriteLine(_tank.Position.ToString());
            //SetPosition(position);
            //_tank.BodyRotation = bodyDirection;
        }

        /// <summary>
        /// Used for debuging network communication. Used to show this OtherPlayer as the main player on the server
        /// </summary>
        /// <param name="position"></param>
        /// <param name="bodyRotation"></param>
        /// <param name="latency"></param>
        /// <param name="mainPlayer"></param>
        public void PlayerGameInfo(Vector2 position, float bodyRotation, float latency, Player mainPlayer)
        {
            while (_tank == null)
                Thread.Sleep(1);

            
            mainPlayer.GetTank().TurnTimeForwardForOldPositionAndBodyRotation(ref position, ref bodyRotation, (float)latency * 2);

            Console.WriteLine("R: " + (bodyRotation - mainPlayer.GetTank().BodyRotation).ToString());

            Console.WriteLine(Vector2.Distance(position, mainPlayer.GetTank().Position));

            _tank.SetPosition(position);
            _tank.BodyRotation = bodyRotation;
        }

        public override void Update(double dt)
        {
            //if (_tankNumber == 2)
            //{
            //    if (KeyboardInput.IsKeyClicked(Keys.NumPad9))
            //        _tank.ActivatePlugin(_tank.Turret.Plugins.Where(i => i.GetType() == typeof(ShieldPlugin)).FirstOrDefault(), Vector2.Zero, null);
            //}
            base.Update(dt);
        }

        public override void Draw(IRender render, Camera camera)
        {
            base.Draw(render, camera);
        }
    }
}
