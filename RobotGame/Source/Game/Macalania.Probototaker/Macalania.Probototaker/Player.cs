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

            Tank t1 = new Tank();

            StarterHull sh = new StarterHull();
            sh.Load(content);
            sh.SetTank(t1);
            t1.SetHull(sh);

            StarterTrack st = new StarterTrack();
            st.Load(content);
            st.SetTank(t1);
            t1.SetTrack(st);

            Turret t = new BigTurret();
            t.Load(content);
            t.SetTank(t1);

            //StarterMainGun smg = new StarterMainGun();
            //smg.Load(content);
            //smg.SetTank(t1);
            //t.AddPluginTop(smg, 1);

            //MiniMainGun mg1 = new MiniMainGun();
            //mg1.Load(content);
            //mg1.SetTank(t1);
            //t.AddPluginTop(mg1, 0);

            SprayMainGun smgg = new SprayMainGun();
            smgg.Load(content);
            smgg.SetTank(t1);
            t.AddPluginTop(smgg,0);

            //MiniMainGun mg2 = new MiniMainGun();
            //mg2.Load(content);
            //mg2.SetTank(t1);
            //t.AddPluginTop(mg2, 1);

            AmorPlugin ap1 = new AmorPlugin(PluginDirection.Left);
            ap1.Load(content);
            ap1.SetTank(t1);
            t.AddPluginLeftSide(ap1, 0);

            AmorPlugin ap2 = new AmorPlugin(PluginDirection.Left);
            ap2.Load(content);
            ap2.SetTank(t1);
            t.AddPluginLeftSide(ap2, 1);

            AmorPlugin ap3 = new AmorPlugin(PluginDirection.Left);
            ap3.Load(content);
            ap3.SetTank(t1);
            t.AddPluginLeftSide(ap3, 2);

            ArtileryStarter art = new ArtileryStarter();
            art.Load(content);
            art.SetTank(t1);
            t.AddPluginButtom(art, 0);

            RocketStarterPlugin r = new RocketStarterPlugin(PluginDirection.Right);
            r.Load(content);
            r.SetTank(t1);
            t.AddPluginRightSide(r, 0);

            t1.SetTurret(t);

            _tank = t1;
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

            if (MouseInput.IsLeftMousePressed())
            {
                _tank.FireMainGun();
            }

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
