using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Plugins
{
    public enum PluginType : byte
    {
        Amor = 0,
        ArtileryStart = 1,
        MinePlayer = 2,
        RocketStarter = 3,
        Shield = 4,
        StarterAntiRocketLaser = 5,
        SunPannel = 9,
        MiniMainGun = 6,
        SprayMainGun = 7,
        StarterMainGun = 8,
        Battery = 10,
        StarterAttackRocket = 11,
    }
    public enum PluginDirection : byte
    {
        NonDirectional = 0,
        Top = 1,
        Buttom = 2,
        Left = 3,
        Right = 4,
    }
    public class Plugin : TankComponent
    {
        public int Size { get; protected set; }
        public int PluginPosition { get; set; }
        public Vector2 OriginOfset { get; set; }
        public float Cooldown { get; set; }
        public float MaxCooldown { get; set; }
        public float PowerUsage { get; set; }

        // Attributes given to the tank
        public float AmorPoints { get; set; }
        public float PowerRegen { get; set; }

        public PluginType PluginType { get; set; }

        public Plugin(PluginType type)
        {
            PluginType = type;
        }

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
