using Macalania.Probototaker.Tanks.Plugins;
using Macalania.Probototaker.Tanks.Plugins.MainGuns;
using Macalania.Probototaker.Tanks.Plugins.Mic;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Turrets
{
    public class TurretModule : TankComponent
    {
        public PluginDirection PluginDir { get; set; }

        protected int _x;
        protected int _y;
        int _dim = 16;

        protected Point _moduleDim;

        public int PluginPosition { get; set; }
        public float Cooldown { get; set; }
        public float MaxCooldown { get; set; }
        public float PowerUsage { get; set; }

        // Attributes given to the tank
        public float AmorPoints { get; set; }
        public float PowerRegen { get; set; }

        public PluginType PluginType { get; set; }

        public List<Point> RequiredBricks { protected get; set; }

        public List<Point> RequiredFreeSpace {protected get;  set; }

        public List<PluginDirection> PossibleDirections { get; set; }


        public TurretModule(PluginType type, Room room) : base(room)
        {
            RequiredBricks = new List<Point>();
            RequiredFreeSpace = new List<Point>();
            PossibleDirections = new List<PluginDirection>();
            PluginType = type;
        }

        public List<Point> GetRotatedFreeSpace()
        {
            if (PluginDir == PluginDirection.Right || PluginDir == PluginDirection.NonDirectional)
                return RequiredFreeSpace;

            List<Point> rotated = new List<Point>();

            if (PluginDir == PluginDirection.Left)
            {
                foreach (Point p in RequiredFreeSpace)
                {
                    rotated.Add(new Point((_moduleDim.X - 1) - p.X,  p.Y));
                }
            }

            return rotated;
        }

        public List<Point> GetRotatedRequiredBricks()
        {
            if (PluginDir == PluginDirection.Right || PluginDir ==  PluginDirection.NonDirectional)
                return RequiredBricks;

            List<Point> rotated = new List<Point>();

            if (PluginDir == PluginDirection.Left)
            {
                foreach (Point p in RequiredBricks)
                {
                    rotated.Add(new Point((_moduleDim.X -1) - p.X,  p.Y));
                }
            }

            return rotated;
        }

        public int GetCentredX()
        {
            return _x - 16;
        }

        public int GetCentredY()
        {
            return _y - 16;
        }

        public int GetX()
        {
            return _x;
        }

        public int GetY()
        {
            return _y;
        }

        public virtual void AddComponents(Turret turret)
        {

        }

        public virtual void SetLocation(int x, int y)
        {
            _x = x;
            _y = y;

            x = x - 16;
            y = y - 16;

            Sprite.Origin = new Vector2(-x * _dim, -y * _dim);
        }

        public override void Update(double dt)
        {
            base.Update(dt);
            Sprite.Rotation = Tank.TurretRotation + Tank.BodyRotation;
            if (Cooldown != 0)
                Cooldown -= (float)dt;
            if (Cooldown < 0)
            {
                Cooldown = 0;
                OnReady();
            }
        }

        public virtual void OnReady()
        {
        }

        protected bool IsActivationValid()
        {
            if (Tank.DoesTankHaveEnoughPower(PowerUsage) && Cooldown == 0)
                return true;
            return false;
        }

        public virtual bool Activate(Vector2 point, Tank target)
        {
            if (IsActivationValid())
            {
                Tank.UsePower(PowerUsage);
                Cooldown = MaxCooldown;
                return true;
            }

            return false;
        }

        public static TurretModule GenerateTurretModule(PluginType type, PluginDirection dir, Room room)
        {
            if (type == PluginType.RocketStarter)
            {
                return new RocketStarterPlugin(dir, room);
            }
            if (type == PluginType.MiniMainGun)
                return new MiniCanon(room);
            if (type == PluginType.ArtileryStart)
                return new ArtileryStarter(room);

            throw new Exception("Turret Module not registred");
        }
    }
}
