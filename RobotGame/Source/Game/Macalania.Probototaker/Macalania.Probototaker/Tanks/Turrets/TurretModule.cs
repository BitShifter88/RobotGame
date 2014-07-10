using Macalania.Probototaker.Tanks.Plugins;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Turrets
{
    public class TurretModule : TankComponent
    {
        protected int _x;
        protected int _y;
        int _dim = 16;

        public int Size { get; protected set; }
        public int PluginPosition { get; set; }
        public float Cooldown { get; set; }
        public float MaxCooldown { get; set; }
        public float PowerUsage { get; set; }

        // Attributes given to the tank
        public float AmorPoints { get; set; }
        public float PowerRegen { get; set; }

        public PluginType PluginType { get; set; }

        public List<Point> RequiredBricks { get; set; }

        public TurretModule(PluginType type)
        {
            RequiredBricks = new List<Point>();
            PluginType = type;
        }

        public int GetCentredX()
        {
            return _x - 16;
        }

        public int GetCentredY()
        {
            return _y - 16;
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
    }
}
