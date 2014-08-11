using Macalania.Probototaker.Tanks;
using Macalania.Probototaker.Tanks.Plugins;
using Macalania.Probototaker.Tanks.Plugins.MainGuns;
using Macalania.Probototaker.Tanks.Plugins.Mic;
using Macalania.Probototaker.Tanks.Turrets;
using Macalania.YunaEngine;
using Macalania.YunaEngine.GameLogic;
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

namespace Macalania.Probototaker.GarageLogic
{
    class TankEditor : GameObject
    {
        public Tank Tank { get; private set; }
        TurretModule _selectedModule;
        TurretBrick _selectedBrick;

        public TankEditor(Room room) : base(room)
        {

        }

        public void SetTank(Tank tank)
        {
            Tank = tank;
        }

        public override void Update(double dt)
        {
            Point mousePos = MousePositionToTurretCoordinate();

            mousePos = ManipulateComponent(mousePos);
            SelectComponent();
            UpdateCursor();
            base.Update(dt);
        }

        private Point ManipulateComponent(Point mousePos)
        {
            if (MouseInput.IsRightMouseClicked())
            {
                RemoveTankComponent();
            }
            if (MouseInput.IsLeftMouseClicked())
            {
                if (_selectedBrick != null && Tank.Turret.CanAddTurretComponent(_selectedBrick, mousePos.X, mousePos.Y))
                {
                    TurretBrick tb = new TurretBrick(Tank);
                    tb.Load(RoomManager.Instance.GetActiveRoom().Content);
                    Tank.Turret.AddTurretComponent(tb, mousePos.X, mousePos.Y);
                    Tank.Turret.DetermineTurretBricks();
                }
                if (_selectedModule != null)
                {
                    if (Tank.Turret.CanAddTurretModule(_selectedModule, mousePos.X, mousePos.Y))
                    {
                        _selectedModule.SetTank(Tank);
                        Tank.Turret.AddTurretModule(_selectedModule, mousePos.X, mousePos.Y);
                        _selectedModule = null;
                    }
                }
            }
            if (KeyboardInput.IsKeyClicked(Keys.R))
            {
                if (_selectedModule != null && _selectedModule.PluginDir != PluginDirection.NonDirectional)
                {
                    byte currentDir = (byte)_selectedModule.PluginDir;

                    while (true)
                    {
                        currentDir++;
                        if (currentDir == 5)
                            currentDir = 1;

                        if (_selectedModule.PossibleDirections.Contains((PluginDirection)currentDir))
                        {
                            TurretModule module = TurretModule.GenerateTurretModule(_selectedModule.PluginType, (PluginDirection)currentDir);
                            module.Load(RoomManager.Instance.GetActiveRoom().Content);
                            module.SetTank(Tank);
                            _selectedModule = module;
                            break;
                        }
                    }

                }
            }
            return mousePos;
        }

        private void SelectComponent()
        {
            if (KeyboardInput.IsKeyClicked(Keys.NumPad0))
            {
                _selectedModule = null;
                _selectedBrick = new TurretBrick(Tank);
                _selectedBrick.Load(RoomManager.Instance.GetActiveRoom().Content);
                _selectedBrick.UseAbsolutePosition = true;
                _selectedBrick.SetLocation(16, 16);
                _selectedBrick.Origin = new Vector2(0, 0);
            }
            else if (KeyboardInput.IsKeyClicked(Keys.NumPad1))
            {
                _selectedBrick = null;
                _selectedModule = new ArtileryStarter();
                _selectedModule.Load(RoomManager.Instance.GetActiveRoom().Content);
            }
            else if (KeyboardInput.IsKeyClicked(Keys.NumPad2))
            {
                _selectedBrick = null;
                RocketStarterPlugin rsp = new RocketStarterPlugin(Tanks.Plugins.PluginDirection.Right);
                rsp.SetTank(Tank);
                rsp.Load(RoomManager.Instance.GetActiveRoom().Content);
                rsp.ReloadRocket();
                _selectedModule = rsp;
            }
            else if (KeyboardInput.IsKeyClicked(Keys.NumPad3))
            {
                _selectedBrick = null;
                _selectedModule = new MiniCanon();
                _selectedModule.Load(RoomManager.Instance.GetActiveRoom().Content);
            }
        }

        private void UpdateCursor()
        {
            Point mousePos = MousePositionToTurretCoordinate();

            if (_selectedBrick != null)
            {
                Vector2 position = new Vector2((MouseInput.X / 16) * 16, (MouseInput.Y / 16) * 16);

                _selectedBrick.AbsolutePosition = position;
            }
            else if (_selectedModule != null)
            {
                Vector2 position = new Vector2((MouseInput.X / 16) * 16, (MouseInput.Y / 16) * 16);
                _selectedModule.Sprite.Position = position;
                _selectedModule.Sprite.Origin = new Vector2(0, 0);

                if (Tank.Turret.CanAddTurretModule(_selectedModule, mousePos.X, mousePos.Y))
                {
                    _selectedModule.SetColor(Color.White);
                }
                else
                    _selectedModule.SetColor(new Color(255, 150, 150, 100));
            }
        }

        private void RemoveTankComponent()
        {
            Point p = MousePositionToTurretCoordinate();
            TurretModule module = Tank.Turret.GetModuleAtLocation(p.X, p.Y);

            if (module != null)
            {
                Tank.Turret.RemoveModule(module);
            }
            else
            {
                if (Tank.Turret.GetTurretComponent(p.X, p.Y) != null && Tank.Turret.GetTurretComponent(p.X, p.Y).GetType() == typeof(TurretBrick))
                {
                    if ((p.X == 15 && p.Y == 15) ||
                        (p.X == 16 && p.Y == 16) ||
                        (p.X == 15 && p.Y == 16) ||
                        (p.X == 16 && p.Y == 15))
                        return;
                    Tank.Turret.RemoveTurretBrick(p.X, p.Y);
                    Tank.Turret.DetermineTurretBricks();
                }
            }
        }

        public override void Draw(IRender render, Camera camera)
        {
            Tank.Draw(render, camera);

            if (_selectedBrick != null)
                _selectedBrick.Draw(render, camera);
            if (_selectedModule != null)
                _selectedModule.Draw(render, camera);
        }

        public Point MousePositionToTurretCoordinate()
        {
            int x = MouseInput.X - (int)Tank.Position.X -16 * 16;
            int y = MouseInput.Y - (int)Tank.Position.Y - 16 * 16;

            x = x / 16 + 16 + 16 - 1;
            y = y / 16 + 16 + 16 - 1;

            return new Point(x, y);
        }
    }
}
