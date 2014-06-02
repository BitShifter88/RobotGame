using Macalania.Probototaker.Tanks;
using Macalania.Probototaker.Tanks.Hulls;
using Macalania.Probototaker.Tanks.Plugins;
using Macalania.Probototaker.Tanks.Plugins.MainGuns;
using Macalania.Probototaker.Tanks.Plugins.Mic;
using Macalania.Probototaker.Tanks.Tracks;
using Macalania.Probototaker.Tanks.Turrets;
using Macalania.YunaEngine.GameLogic;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker
{
    class OtherPlayer : GameObject
    {
        Tank _tank;
        ShieldPlugin sp;
        RocketStarterPlugin r;
        ArtileryStarter art;
        public override void Inizialize()
        {
            base.Inizialize();
        }

        public override void Load(ContentManager content)
        {
            Tank t1 = new Tank( new Vector2(800,200));

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

            StarterMainGun smg = new StarterMainGun();
            smg.Load(content);
            smg.SetTank(t1);
            t.AddPluginTop(smg, 1);

            //MiniMainGun mg1 = new MiniMainGun();
            //mg1.SetTank(t1);
            //mg1.Load(content);
            //t.AddPluginTop(mg1, 0);

            //MiniMainGun mg2 = new MiniMainGun();
            //mg2.SetTank(t1);
            //mg2.Load(content);
            //t.AddPluginTop(mg2, 1);

            //MiniMainGun mg3 = new MiniMainGun();
            //mg3.SetTank(t1);
            //mg3.Load(content);
            //t.AddPluginTop(mg3, 2);

            //SprayMainGun smgg = new SprayMainGun(t);
            //smgg.Load(content);
            //smgg.SetTank(t1);
            //t.AddPluginTop(smgg,0);

            MiniMainGun mg2 = new MiniMainGun();
            mg2.Load(content);
            mg2.SetTank(t1);
            t.AddPluginTop(mg2, 0);

            AmorPlugin ap1 = new AmorPlugin(PluginDirection.Left);
            ap1.Load(content);
            ap1.SetTank(t1);
            t.AddPluginLeftSide(ap1, 0);

            //SunPannelPlugin spp = new SunPannelPlugin(PluginDirection.Left);
            //spp.Load(content);
            //spp.SetTank(t1);
            //t.AddPluginLeftSide(spp, 0);

            AmorPlugin ap2 = new AmorPlugin(PluginDirection.Left);
            ap2.Load(content);
            ap2.SetTank(t1);
            t.AddPluginLeftSide(ap2, 1);

            //BatteryPlugin bt = new BatteryPlugin(PluginDirection.Left);
            //bt.Load(content);
            //bt.SetTank(t1);
            //t.AddPluginLeftSide(bt, 1);

            AmorPlugin ap3 = new AmorPlugin(PluginDirection.Left);
            ap3.Load(content);
            ap3.SetTank(t1);
            t.AddPluginLeftSide(ap3, 2);

            //sp = new ShieldPlugin(PluginDirection.Left);
            //sp.Load(content);
            //sp.SetTank(t1);
            //t.AddPluginLeftSide(sp, 2);

            art = new ArtileryStarter();
            art.Load(content);
            art.SetTank(t1);
            t.AddPluginButtom(art, 0);

            r = new RocketStarterPlugin(PluginDirection.Right);
            r.SetTank(t1);
            r.Load(content);
            t.AddPluginRightSide(r, 0);



            _tank = t1;

            _tank.ReadyTank();

            base.Load(content);
        }

        public override void Update(double dt)
        {
            base.Update(dt);

            _tank.Update(dt);
        }

        public override void Draw(IRender render, Camera camera)
        {
            base.Draw(render, camera);

            _tank.Draw(render, camera);
        }
    }
}
