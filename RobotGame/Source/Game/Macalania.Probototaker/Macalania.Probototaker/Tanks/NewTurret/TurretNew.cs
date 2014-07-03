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
        private List<TurretModule> _modules = new List<TurretModule>();

        public TurretNew()
        {
            _turretComponents = new TurretComponent[64, 64];

        }

        public void FireMainGun()
        {
            List<MainGunNew> mgs = GetMainGuns();

            foreach (MainGunNew mg in mgs)
            {
                mg.FireRequest(this);
            }
        }

        private List<MainGunNew> GetMainGuns()
        {
            List<MainGunNew> mainGuns = new List<MainGunNew>();

            foreach (TurretModule p in _modules)
            {
                if (p.GetType().IsSubclassOf(typeof(MainGunNew)))
                {
                    mainGuns.Add((MainGunNew)p);
                }
            }

            return mainGuns;
        }

        public void AddTurretModule(TurretModule module, int x, int y)
        {
            _modules.Add(module);
            module.SetLocation(x, y);
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

        public void Update(double dt)
        {
            foreach (TurretModule module in _modules)
            {
                module.Update(dt);
            }
        }

        public void Draw(IRender render, Camera camera)
        {
            foreach (TurretComponent tc in _turretComponents)
            {
                if (tc == null)
                    continue;
                tc.Draw(render, camera);
            }

            foreach (TurretModule module in _modules)
            {
                module.Draw(render, camera);
            }
        }
    }
}
