using Macalania.Probototaker.GarageLogic;
using Macalania.Probototaker.Tanks;
using Macalania.Robototaker.Protocol;
using Macalania.YunaEngine;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Gui;
using Macalania.YunaEngine.Input;
using Macalania.YunaEngine.Rendering;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Rooms
{
    class Garage : Room
    {
        TankEditor _editor;
        MainFrameConnection _connection;
        GuiSystem _gui;

        public Garage(MainFrameConnection connection)
        {
            _connection = connection;
        }

        public override void Load(IServiceProvider serviceProvider)
        {
            base.Load(serviceProvider);

            _gui = new GuiSystem(this);
            _gui.AddGuiComponent(new StockImage(Content.LoadYunaTexture("Textures/Garage/menubackground"), new Vector2(0, 100)));
            _gui.AddGuiComponent(new StockImage(Content.LoadYunaTexture("Textures/Garage/battle"), new Vector2(1920/2 -400 / 2, 0)));

            AddGameObject(_gui);

            Camera = new Camera();
            _editor = new TankEditor(this);
            Tank tank = TankGenerator.GenerateStarterTank(Content, new Vector2(((Globals.Viewport.Width / 2) / 16) * 16, ((Globals.Viewport.Height / 2) / 16) * 16), this);
            tank.ReadyTank(this);
            _editor.SetTank(tank);

            AddGameObject(_editor);


        }

        public override void Update(double dt)
        {
            YunaEngine.YunaGameEngine.Instance.Window.Title = _editor.MousePositionToTurretCoordinate().ToString();

            if (KeyboardInput.IsKeyClicked(Keys.Enter))
            {
                //LoadGameRoom lgr = new LoadGameRoom(_editor.Tank.GetTankPackage());
                QueRoom qr = new QueRoom(_connection, _editor.Tank.GetTankPackage());
                YunaGameEngine.Instance.SetActiveRoom(qr, true);
            }

            base.Update(dt);
        }

    }
}
