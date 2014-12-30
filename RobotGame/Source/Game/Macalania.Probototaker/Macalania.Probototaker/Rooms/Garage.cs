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
    enum ActiveWindow
    {
        None,
        Tank,
        Technology,
    }

    class Garage : Room
    {
        MainFrameConnection _connection;
        ActiveWindow _window = ActiveWindow.None;
        
        // Garage
        TankEditor _editor;
        Tank _tank;
        StockImage _garageButton;

        // Technology
        StockImage _techWindow;
        StockImage _technologyButton;
        StockImage _gunsButton;
        StockImage _rocketsButton;
        StockImage _specialButton;
        StockImage _amorButton;
        StockImage _chassisButton;
        StockImage _tracksButton;

        public Garage(MainFrameConnection connection)
        {
            _connection = connection;
        }

        public override void Load(IServiceProvider serviceProvider)
        {
            base.Load(serviceProvider);

            Gui.AddGuiComponent(new StockImage(Content.LoadYunaTexture("Textures/Garage/menubackground"), new Vector2(0, 100)));
            Gui.AddGuiComponent(new StockImage(Content.LoadYunaTexture("Textures/Garage/battle"), new Vector2(1920 / 2 - 400 / 2, 0)));
            Gui.AddGuiComponent(new StockImage(Content.LoadYunaTexture("Textures/Garage/store"), new Vector2(350 + 300 + 300, 130)));
            Gui.AddGuiComponent(new StockImage(Content.LoadYunaTexture("Textures/Garage/social"), new Vector2(350 + 300 + 300 + 300, 130)));

            _garageButton = new StockImage(Content.LoadYunaTexture("Textures/Garage/garage"), new Vector2(350, 130));
            Gui.AddGuiComponent(_garageButton);

            _technologyButton = new StockImage(Content.LoadYunaTexture("Textures/Garage/technology"), new Vector2(350 + 300, 130));
            Gui.AddGuiComponent(_technologyButton);

            // Technology
            _techWindow = new StockImage(Content.LoadYunaTexture("Textures/Garage/technologywindow"), new Vector2(272, 264));
            _gunsButton = new StockImage(Content.LoadYunaTexture("Textures/Garage/guns"), new Vector2(272, 264));
            _rocketsButton = new StockImage(Content.LoadYunaTexture("Textures/Garage/rockets"), new Vector2(272, 264 + 100));
            _specialButton = new StockImage(Content.LoadYunaTexture("Textures/Garage/special"), new Vector2(272, 264 + 200));
            _amorButton = new StockImage(Content.LoadYunaTexture("Textures/Garage/amor"), new Vector2(272, 264 + 300));
            _chassisButton = new StockImage(Content.LoadYunaTexture("Textures/Garage/chassis"), new Vector2(272, 264 + 400));
            _tracksButton = new StockImage(Content.LoadYunaTexture("Textures/Garage/tracks"), new Vector2(272, 264 + 500));

            Camera = new Camera();
            _editor = new TankEditor(this);
            _tank = TankGenerator.GenerateStarterTank(Content, new Vector2(((Globals.Viewport.Width / 2) / 16) * 16, ((Globals.Viewport.Height / 2) / 16) * 16), this);
            _tank.ReadyTank(this);
            _editor.SetTank(_tank);

            GotoGarage();
        }

        private void GotoGarage()
        {
            if (_window == ActiveWindow.Tank)
                return;

            RemoveTechnology();

            AddGameObjectWhileRunning(_editor);
            AddGameObjectWhileRunning(_tank);

            _window = ActiveWindow.Tank;
        }

        private void GotoTechnology()
        {
            if (_window == ActiveWindow.Technology)
                return;

            RemoveGarage();

            Gui.AddGuiComponent(_techWindow);
            Gui.AddGuiComponent(_gunsButton);
            Gui.AddGuiComponent(_rocketsButton);
            Gui.AddGuiComponent(_specialButton);
            Gui.AddGuiComponent(_amorButton);
            Gui.AddGuiComponent(_chassisButton);
            Gui.AddGuiComponent(_tracksButton);

            _window = ActiveWindow.Technology;
        }

        private void RemoveGarage()
        {
            RemoveGameObject(_editor);
            RemoveGameObject(_tank);
        }

        private void RemoveTechnology()
        {
            Gui.RemoveGuiComponent(_techWindow);
            Gui.RemoveGuiComponent(_gunsButton);
            Gui.RemoveGuiComponent(_rocketsButton);
            Gui.RemoveGuiComponent(_specialButton);
            Gui.RemoveGuiComponent(_amorButton);
            Gui.RemoveGuiComponent(_chassisButton);
            Gui.RemoveGuiComponent(_tracksButton);
        }

        public override void Update(double dt)
        {
            YunaEngine.YunaGameEngine.Instance.Window.Title = _editor.MousePositionToTurretCoordinate().ToString();

            HandleShortCuts();
            HandleButtons();

            base.Update(dt);
        }

        private void HandleButtons()
        {
            if (_technologyButton.Clicked)
                GotoTechnology();

            if (_garageButton.Clicked)
                GotoGarage();
        }

        private void HandleShortCuts()
        {
            if (KeyboardInput.IsKeyClicked(Keys.Enter))
            {
                //LoadGameRoom lgr = new LoadGameRoom(_editor.Tank.GetTankPackage());
                QueRoom qr = new QueRoom(_connection, _editor.Tank.GetTankPackage());
                YunaGameEngine.Instance.SetActiveRoom(qr, true);
            }
            if (KeyboardInput.IsKeyClicked(Keys.T))
            {
                GotoTechnology();
            }
            if (KeyboardInput.IsKeyClicked(Keys.G))
            {
                GotoGarage();
            }
        }

    }
}
