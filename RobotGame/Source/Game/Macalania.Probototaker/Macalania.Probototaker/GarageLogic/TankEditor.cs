using Macalania.Probototaker.Tanks;
using Macalania.Probototaker.Tanks.Turrets;
using Macalania.YunaEngine.GameLogic;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Input;
using Macalania.YunaEngine.Rendering;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.GarageLogic
{
    class TankEditor : GameObject
    {
        public Tank Tank { get; private set; }

        public TankEditor(Room room) : base(room)
        {

        }

        public void SetTank(Tank tank)
        {
            Tank = tank;
        }

        public override void Update(double dt)
        {
            if (MouseInput.IsRightMouseClicked())
            {
                RemoveTankComponent();
            }
            base.Update(dt);
        }

        private void RemoveTankComponent()
        {
            Point p = MousePositionToTurretCoordinate();
            TurretModule module = Tank.Turret.GetModuleAtLocation(p.X, p.Y);

            if (module != null)
            {
                Tank.Turret.RemoveModule(module);
            }
        }

        public override void Draw(IRender render, Camera camera)
        {
            Tank.Draw(render, camera);
        }

        public Point MousePositionToTurretCoordinate()
        {
            int x = MouseInput.X - (int)Tank.Position.X -16 * 16;
            int y = MouseInput.Y - (int)Tank.Position.Y - 16 * 16;

            x = x / 16 + 16 + 16;
            y = y / 16 + 16 + 16;

            return new Point(x, y);
        }
    }
}
