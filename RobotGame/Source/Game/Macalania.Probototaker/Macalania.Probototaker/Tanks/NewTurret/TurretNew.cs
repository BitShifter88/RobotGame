using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.NewTurret
{
    public class TurretNew
    {
        private TurretComponent[,] _turretComponents;

        public TurretNew()
        {
            _turretComponents = new TurretComponent[64, 64];

        }

        public void AddTurretComponent(TurretComponent component, int x, int y)
        {
            _turretComponents[x, y] = component;
            component.SetLocation(x, y);
        }

        public bool CanAddTurretComponent(TurretComponent component, int x, int y)
        {
            return false;
        }

        public void Draw(IRender render, Camera camera)
        {
            foreach (TurretComponent tc in _turretComponents)
            {
                if (tc == null)
                    continue;
                tc.Draw(render, camera);
            }
        }
    }
}
