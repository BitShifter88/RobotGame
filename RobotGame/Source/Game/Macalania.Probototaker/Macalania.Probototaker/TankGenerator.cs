using Macalania.Probototaker.Tanks;
using Macalania.Probototaker.Tanks.Hulls;
using Macalania.Probototaker.Tanks.Turrets;
using Macalania.Probototaker.Tanks.Plugins;
using Macalania.Probototaker.Tanks.Plugins.Mic;
using Macalania.Probototaker.Tanks.Tracks;
using Macalania.YunaEngine.Resources;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Macalania.Probototaker.Tanks.Plugins.MainGuns;

namespace Macalania.Probototaker
{
    public class TankGenerator
    {
        public static Tank GenerateStarterTank(ResourceManager content, Vector2 position, Room room)
        {
            Tank t1 = new Tank(room, position);

            StarterHull sh = new StarterHull(room);
            sh.SetTank(t1);
            sh.Load(content);
            t1.SetHull(sh);

            StarterTrack st = new StarterTrack(room);
            st.SetTank(t1);
            st.Load(content);
            t1.SetTrack(st);

            Turret t = new Turret(t1, room);

            t1.SetTurret(t);

            for (int i = 16 - 2; i < 16 + 2; i++)
            {
                for (int j = 16 - 1; j < 16 + 2; j++)
                {
                    TurretBrick tb = new TurretBrick(t1, room);
                    tb.Load(room.Content);
                    t.AddTurretComponent(tb, i, j);
                }
            }

            MiniCanon m = new MiniCanon(room);
            m.Load(content);
            m.SetTank(t1);

            t.AddTurretModule(m, 16-2, 12);

            t1.TurretStyle = new ClasicStyle(content);

            t.DetermineTurretBricks();

            return t1;
        }

        public static Tank GenerateTank3(Room room, ResourceManager content, Vector2 position)
        {
            Tank t1 = new Tank(room, position);

            StarterHull sh = new StarterHull(room);
            sh.SetTank(t1);
            sh.Load(content);
            t1.SetHull(sh);

            StarterTrack st = new StarterTrack(room);
            st.SetTank(t1);
            st.Load(content);
            t1.SetTrack(st);

            Turret t = new Turret(t1, room);

            t1.SetTurret(t);

            for (int i = 16 - 2; i < 16 + 2; i++)
            {
                for (int j = 16 - 1; j < 16 + 4; j++)
                {
                    TurretBrick tb = new TurretBrick(t1, room);
                    tb.Load(room.Content);
                    t.AddTurretComponent(tb, i, j);
                }
            }

            MiniCanon m = new MiniCanon(room);
            m.Load(content);
            m.SetTank(t1);

            t.AddTurretModule(m, 16 - 2, 12);

            MiniCanon m2 = new MiniCanon(room);
            m2.Load(content);
            m2.SetTank(t1);

            t.AddTurretModule(m2, 16, 12);

            //StarterMainGun smg = new StarterMainGun();
            //smg.Load(content);
            //smg.SetTank(t1);
            //t.AddPluginTop(smg, 1);

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

            //MiniMainGun mg2 = new MiniMainGun();
            //mg2.Load(content);
            //mg2.SetTank(t1);
            //t.AddPluginTop(mg2, 1);

            //AmorPlugin ap1 = new AmorPlugin(PluginDirection.Left);
            //ap1.Load(content);
            //ap1.SetTank(t1);
            //t.AddPluginLeftSide(ap1, 0);

            //SunPannelPlugin spp = new SunPannelPlugin(PluginDirection.Left);
            //spp.Load(content);
            //spp.SetTank(t1);
            //t.AddPluginLeftSide(spp, 0);

            ////AmorPlugin ap2 = new AmorPlugin(PluginDirection.Left);
            ////ap2.Load(content);
            ////ap2.SetTank(t1);
            ////t.AddPluginLeftSide(ap2, 1);

            //BatteryPlugin bt = new BatteryPlugin(PluginDirection.Left);
            //bt.Load(content);
            //bt.SetTank(t1);
            //t.AddPluginLeftSide(bt, 1);

            //AmorPlugin ap3 = new AmorPlugin(PluginDirection.Right);
            //ap3.Load(content);
            //ap3.SetTank(t1);
            //t.AddPluginRightSide(ap3, 2);

            //ShieldPlugin sp = new ShieldPlugin(PluginDirection.Left);
            //sp.Load(content);
            //sp.SetTank(t1);
            //t.AddPluginLeftSide(sp, 2);

            //ArtileryStarter art = new ArtileryStarter();
            //art.Load(content);
            //art.SetTank(t1);
            //t.AddPluginButtom(art, 0);

            //mlp = new MineLayerPlugin();
            //mlp.Load(content);
            //mlp.SetTank(t1);
            //t.AddPluginButtom(mlp, 0);

            //StarterAttackRocketBatteryPlugin attack = new StarterAttackRocketBatteryPlugin();
            //attack.SetTank(t1);
            //attack.Load(content);
            //t.AddPluginRightSide(attack, 0);

            t1.ReadyTank(room);

            t.DetermineTurretBricks();

            return t1;
        }

        public static Tank GenerateTank2(Room room, ResourceManager content, Vector2 position)
        {
            Tank t1 = new Tank(room,position);

            StarterHull sh = new StarterHull(room);
            sh.SetTank(t1);
            sh.Load(content);
            t1.SetHull(sh);

            StarterTrack st = new StarterTrack(room);
            st.SetTank(t1);
            st.Load(content);
            t1.SetTrack(st);

            Turret t = new Turret(t1, room);
            t1.SetTurret(t);

            for (int i = 16 - 2; i < 16 + 2; i++)
            {
                for (int j = 16 - 1; j < 16 + 4; j++)
                {
                    TurretBrick tb = new TurretBrick(t1, room);
                    tb.Load(room.Content);
                    t.AddTurretComponent(tb, i, j);
                }
            }

            MiniCanon m = new MiniCanon(room);
            m.Load(content);
            m.SetTank(t1);

            t.AddTurretModule(m, 16 - 2, 12);

            MiniCanon m2 = new MiniCanon(room);
            m2.Load(content);
            m2.SetTank(t1);

            t.AddTurretModule(m2, 16, 12);

            //StarterMainGun smg = new StarterMainGun();
            //smg.Load(content);
            //smg.SetTank(t1);
            //t.AddPluginTop(smg, 0);


            //AmorPlugin ap1 = new AmorPlugin(PluginDirection.Left);
            //ap1.Load(content);
            //ap1.SetTank(t1);
            //t.AddPluginLeftSide(ap1, 0);

            //AmorPlugin ap2 = new AmorPlugin(PluginDirection.Left);
            //ap2.Load(content);
            //ap2.SetTank(t1);
            //t.AddPluginLeftSide(ap2, 1);

            //AmorPlugin ap3 = new AmorPlugin(PluginDirection.Left);
            //ap3.Load(content);
            //ap3.SetTank(t1);
            //t.AddPluginLeftSide(ap3, 2);


            //AmorPlugin ap4 = new AmorPlugin(PluginDirection.Right);
            //ap4.Load(content);
            //ap4.SetTank(t1);
            //t.AddPluginRightSide(ap4, 0);

            //AmorPlugin ap5 = new AmorPlugin(PluginDirection.Right);
            //ap5.Load(content);
            //ap5.SetTank(t1);
            //t.AddPluginRightSide(ap5, 1);

            //AmorPlugin ap6 = new AmorPlugin(PluginDirection.Right);
            //ap6.Load(content);
            //ap6.SetTank(t1);
            //t.AddPluginRightSide(ap6, 2);


            //AmorPlugin ap7 = new AmorPlugin(PluginDirection.Buttom);
            //ap7.Load(content);
            //ap7.SetTank(t1);
            //t.AddPluginButtom(ap7, 0);

            //ShieldPlugin sp = new ShieldPlugin(PluginDirection.Buttom);
            //sp.Load(content);
            //sp.SetTank(t1);
            //t.AddPluginButtom(sp, 1);

            t1.ReadyTank(room);

            t.DetermineTurretBricks();

            return t1;
        }

        public static Tank GenerateTank1(Room room, ResourceManager content, Vector2 position)
        {
            Tank t1 = new Tank(room, position);

            StarterHull sh = new StarterHull(room);
            sh.SetTank(t1);
            sh.Load(content);
            t1.SetHull(sh);

            StarterTrack st = new StarterTrack(room);
            st.SetTank(t1);
            st.Load(content);
            t1.SetTrack(st);

            Turret t = new Turret(t1, room);

            t1.SetTurret(t);

            for (int i = 16 - 2; i < 16 + 2; i++)
            {
                for (int j = 16 - 1; j < 16 + 4; j++)
                {
                    TurretBrick tb = new TurretBrick(t1, room);
                    tb.Load(room.Content);
                    t.AddTurretComponent(tb, i, j);
                }
            }

            MiniCanon m = new MiniCanon(room);
            m.Load(content);
            m.SetTank(t1);

            t.AddTurretModule(m, 16 - 2, 12);

            MiniCanon m2 = new MiniCanon(room);
            m2.Load(content);
            m2.SetTank(t1);

            t.AddTurretModule(m2, 16, 12);

            //StarterMainGun smg = new StarterMainGun();
            //smg.Load(content);
            //smg.SetTank(t1);
            //t.AddPluginTop(smg, 1);

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

            //MiniMainGun mg2 = new MiniMainGun();
            //mg2.Load(content);
            //mg2.SetTank(t1);
            //t.AddPluginTop(mg2, 0);

            //AmorPlugin ap1 = new AmorPlugin(PluginDirection.Left);
            //ap1.Load(content);
            //ap1.SetTank(t1);
            //t.AddPluginLeftSide(ap1, 0);

            ////SunPannelPlugin spp = new SunPannelPlugin(PluginDirection.Left);
            ////spp.Load(content);
            ////spp.SetTank(t1);
            ////t.AddPluginLeftSide(spp, 0);

            //AmorPlugin ap2 = new AmorPlugin(PluginDirection.Left);
            //ap2.Load(content);
            //ap2.SetTank(t1);
            //t.AddPluginLeftSide(ap2, 1);

            ////BatteryPlugin bt = new BatteryPlugin(PluginDirection.Left);
            ////bt.Load(content);
            ////bt.SetTank(t1);
            ////t.AddPluginLeftSide(bt, 1);

            //AmorPlugin ap3 = new AmorPlugin(PluginDirection.Left);
            //ap3.Load(content);
            //ap3.SetTank(t1);
            //t.AddPluginLeftSide(ap3, 2);

            ////sp = new ShieldPlugin(PluginDirection.Left);
            ////sp.Load(content);
            ////sp.SetTank(t1);
            ////t.AddPluginLeftSide(sp, 2);

            ////ArtileryStarter art = new ArtileryStarter();
            ////art.Load(content);
            ////art.SetTank(t1);
            ////t.AddPluginButtom(art, 0);

            //RocketStarterPlugin r = new RocketStarterPlugin(PluginDirection.Right);
            //r.SetTank(t1);
            //r.Load(content);
            //t.AddPluginRightSide(r, 0);



            t1.ReadyTank(room);
            t.DetermineTurretBricks();
            return t1;
        }
    }
}
