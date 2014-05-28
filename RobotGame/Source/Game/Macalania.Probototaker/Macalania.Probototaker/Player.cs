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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker
{
    class Player : GameObject
    {
        Tank _tank;

        public Player()
        {

        }
        public override void Inizialize()
        {


            base.Inizialize();
        }
        public override void Load(ContentManager content)
        {
            base.Load(content);

            _tank = new Tank();

            StarterHull sh = new StarterHull();
            sh.Load(content);
            sh.SetTank(_tank);
            _tank.SetHull(sh);

            StarterTrack st = new StarterTrack();
            st.Load(content);
            st.SetTank(_tank);
            _tank.SetTrack(st);

            Turret t = new StarterTurret();
            t.Load(content);
            t.SetTank(_tank);

            StarterMainGun smg = new StarterMainGun();
            smg.Load(content);
            smg.SetTank(_tank);
            t.AddPluginTop(smg, 0);

            AmorPlugin ap1 = new AmorPlugin(PluginDirection.Left);
            ap1.Load(content);
            ap1.SetTank(_tank);
            t.AddPluginLeftSide(ap1, 0);

            AmorPlugin ap2 = new AmorPlugin(PluginDirection.Left);
            ap2.Load(content);
            ap2.SetTank(_tank);
            t.AddPluginLeftSide(ap2, 1);

            AmorPlugin ap3 = new AmorPlugin(PluginDirection.Left);
            ap3.Load(content);
            ap3.SetTank(_tank);
            t.AddPluginLeftSide(ap3, 2);

            RocketStarterPlugin r = new RocketStarterPlugin(PluginDirection.Right);
            r.Load(content);
            r.SetTank(_tank);
            t.AddPluginRightSide(r, 0);
           
            _tank.SetTurret(t);
        }
        private void HandleInput()
        {
            if (KeyboardInput.IsKeyDown(Keys.A))
            {
                _tank.RotateBody(RotationDirection.CounterClockWise);
            }
            if (KeyboardInput.IsKeyDown(Keys.D))
            {
                _tank.RotateBody(RotationDirection.ClockWise);
            }
            if (KeyboardInput.IsKeyDown(Keys.W))
                _tank.Forward();
            if (KeyboardInput.IsKeyDown(Keys.S))
                _tank.Backwards();

            _tank.MoveTurretTowardsPoint(new Vector2(MouseInput.X, MouseInput.Y));
        }
        public override void Update(double dt)
        {
            base.Update(dt);

            HandleInput();
            _tank.Update(dt);
        }
        public override void Draw(IRender render, Camera camera)
        {
            base.Draw(render, camera);

            _tank.Draw(render, camera);
        }
    }
}
