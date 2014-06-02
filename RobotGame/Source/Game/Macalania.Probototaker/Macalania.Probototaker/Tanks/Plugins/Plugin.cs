using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Plugins
{
    enum PluginDirection
    {
        Top,
        Buttom,
        Left,
        Right,
    }
    class Plugin : TankComponent
    {
        public int Size { get; protected set; }
        public int PluginPosition { get; set; }
        public Vector2 OriginOfset { get; set; }
        public float Cooldown { get; set; }
        public float MaxCooldown { get; set; }
        public float PowerUsage { get; set; }

        public void SetOriginFromTurret(Vector2 origin)
        {

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
