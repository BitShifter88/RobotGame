using Macalania.Probototaker.GarageLogic;
using Macalania.Probototaker.Tanks;
using Macalania.Robototaker.Protocol;
using Macalania.YunaEngine;
using Macalania.YunaEngine.Graphics;
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

        public override void Load(IServiceProvider serviceProvider)
        {
            base.Load(serviceProvider);

            Camera = new Camera();
            _editor = new TankEditor(this);
            Tank tank = TankGenerator.GenerateStarterTank(Content, new Vector2(((Globals.Viewport.Width / 2) / 16) * 16, ((Globals.Viewport.Height / 2) / 16) * 16), this);
            tank.ReadyTank(this);
            _editor.SetTank(tank);

            AddGameObject(_editor);

            _connection = new MainFrameConnection();
            _connection.Connect();
        }

        public override void Update(double dt)
        {
            YunaEngine.YunaGameEngine.Instance.Window.Title = _editor.MousePositionToTurretCoordinate().ToString();

            if (KeyboardInput.IsKeyClicked(Keys.Enter))
            {
                LoadGameRoom lgr = new LoadGameRoom(_editor.Tank.GetTankPackage());
                YunaGameEngine.Instance.SetActiveRoom(lgr, true, YunaGameEngine.Instance.Services);
            }

            base.Update(dt);
        }

    }
}
