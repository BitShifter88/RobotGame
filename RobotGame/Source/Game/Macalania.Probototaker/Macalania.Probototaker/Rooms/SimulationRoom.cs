using Macalania.Probototaker.Effects;
using Macalania.Probototaker.Projectiles;
using Macalania.Probototaker.Tanks;
using Macalania.YunaEngine.GameLogic;
using Macalania.YunaEngine.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Rooms
{
    public class SimulationRoom : Room
    {
        public List<Tank> Tanks { get; set; }
        public List<Shield> Shields { get; set; }
        public List<Projectile> Projectiles { get; set; }

        public SimulationRoom()
        {
            Tanks = new List<Tank>();
            Shields = new List<Shield>();
        }

        protected override void RemoveGameObject(GameObject obj)
        {
            if (obj.GetType() == typeof(Tank))
                Tanks.Remove((Tank)obj);
            if (obj.GetType() == typeof(Shield))
                Shields.Remove((Shield)obj);

            base.RemoveGameObject(obj);
        }

        public virtual void RegisterProjectileFiering(Projectile proj, Tank source)
        {

        }

        public override void AddGameObject(GameObject obj)
        {
            RegisterGameObject(obj);

            base.AddGameObject(obj);
        }

        protected virtual void RegisterGameObject(GameObject obj)
        {
            if (obj.GetType() == typeof(Tank))
            {
                Tanks.Add((Tank)obj);
            }
            if (obj.GetType() == typeof(Shield))
            {
                Shields.Add((Shield)obj);
            }
            if (obj.GetType() == typeof(Projectile))
            {
                Projectiles.Add((Projectile)obj);
            }
        }

        public override void AddGameObjectWhileRunning(GameObject obj)
        {
            RegisterGameObject(obj);

            base.AddGameObjectWhileRunning(obj);
        }
    }
}
