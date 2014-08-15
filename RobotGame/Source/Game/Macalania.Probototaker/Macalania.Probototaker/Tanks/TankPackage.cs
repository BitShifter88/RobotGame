using Lidgren.Network;
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

        public static TankPackage ReadTankPackage(NetIncomingMessage message)
        {
            TankPackage tp = new TankPackage();

            byte turretBricks = message.ReadByte();
            byte modules = message.ReadByte();

            for (int i = 0; i < turretBricks; i++)
            {
                byte x = message.ReadByte();
                byte y = message.ReadByte();

                tp.TurretBrickPackages.Add(new TurretBrickPackage() { X = x, Y = y });
            }

            for (int j = 0; j < modules; j++)
            {
                PluginType type = (PluginType)message.ReadByte();
                PluginDirection dir = (PluginDirection)message.ReadByte();
                byte x = message.ReadByte();
                byte y = message.ReadByte();

                tp.ModulePackages.Add(new ModulePackage() { ModuleType = type, ModuleDir = dir, X = x, Y = y });
            }

            return tp;
        }

        public void WriteToMessage(NetOutgoingMessage message)
        {
            message.Write((byte)TurretBrickPackages.Count);
            message.Write((byte)ModulePackages.Count);

            foreach (TurretBrickPackage b in TurretBrickPackages)
            {
                message.Write(b.X);
                message.Write(b.Y);
            }

            foreach (ModulePackage m in ModulePackages)
            {
                message.Write((byte)m.ModuleType);
                message.Write((byte)m.ModuleDir);
                message.Write(m.X);
                message.Write(m.Y);
            }
        }
    }
}
