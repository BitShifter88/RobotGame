using Macalania.Probototaker.Tanks.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks
{
    public class ModulePackage
    {
        public PluginType ModuleType { get; set; }
        public PluginDirection ModuleDir { get; set; }
        public byte X { get; set; }
        public byte Y { get; set; }
    }
    public class TurretBrickPackage
    {
        public byte X { get; set; }
        public byte Y { get; set; }
    }
    public class TankPackage
    {
        public List<ModulePackage> ModulePackages { get; set; }
        public List<TurretBrickPackage> TurretBrickPackages { get; set; }

        public TankPackage()
        {
            ModulePackages = new List<ModulePackage>();
            TurretBrickPackages = new List<TurretBrickPackage>();
        }
    }
}
