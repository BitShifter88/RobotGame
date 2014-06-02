using Macalania.Probototaker.Tanks.Hulls;
using Macalania.Probototaker.Tanks.Plugins;
using Macalania.Probototaker.Tanks.Tracks;
using Macalania.Probototaker.Tanks.Turrets;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Macalania.Probototaker.Tanks
{
    enum RotationDirection
    {
        CounterClockWise,
        ClockWise,
    }

    class Tank
    {
        public Vector2 Position { get; private set; }
        public Hull Hull { get; private set; }
        public Track Track { get; private set; }
        public Turret Turret { get; private set; }
        public float BodyRotation { get; set; }
        public float TurretRotation { get; set; }
        public float CurrentPower { get; set; }
        public float CurrentHp { get; set; }

        public Tank(Vector2 position)
        {
            Position = position;
        }

        public void ReadyTank()
        {
            CurrentPower = GetMaxPower();
            CurrentHp = GetMaxHp();
        }

        private Vector2 GetBodyDirection()
        {
            return new Vector2((float)Math.Cos((double)BodyRotation - MathHelper.ToRadians(90)), (float)Math.Sin((double)BodyRotation - MathHelper.ToRadians(90)));
        }

        public  Vector2 GetTurretDirection()
        {
            return new Vector2((float)Math.Cos((double)TurretRotation + BodyRotation + MathHelper.ToRadians(90)), (float)Math.Sin((double)TurretRotation + BodyRotation + MathHelper.ToRadians(90)));
        }

        public float GetTurrentBodyRotation()
        {
            return TurretRotation + BodyRotation + MathHelper.ToRadians(180);
        }

        public void SetHull(Hull hull)
        {
            Hull = hull;
        }

        public void SetTrack(Track track)
        {
            Track = track;
        }

        public void SetTurret(Turret turret)
        {
            Turret = turret;
        }

        public void FireMainGun()
        {
            Turret.FireMainGun();
        }

        public void Forward()
        {
            Position += GetBodyDirection();
        }

        public void Backwards()
        {
            Position -= GetBodyDirection();
        }

        int counter = 0;
        public void MoveTurretTowardsPoint(Vector2 point)
        {
            Vector2 turretRotation = GetTurretDirection();
            Vector2 pointDir = Position - point;
            pointDir.Normalize();

            float angle = (float)Math.Acos((float)Vector2.Dot(pointDir, turretRotation));

            Vector2 turretRotationAfterExtra = new Vector2((float)Math.Cos((double)TurretRotation + 0.01f + BodyRotation + MathHelper.ToRadians(90)), (float)Math.Sin((double)TurretRotation + 0.01f + BodyRotation + MathHelper.ToRadians(90)));
            float angleAfterExtra = (float)Math.Acos((float)Vector2.Dot(pointDir, turretRotationAfterExtra));

            if (angleAfterExtra > angle)
            {
                if (angle > 0.005f)
                    TurretRotation -= 0.01f;
            }
            else
            {
                if (angle > 0.005f)
                    TurretRotation += 0.01f;
            }
        }

        public void RotateBody(RotationDirection dir)
        {
            if (dir == RotationDirection.ClockWise)
                BodyRotation += 0.03f;
            else
                BodyRotation -= 0.03f;
        }
        public void Update(double dt)
        {
            Hull.Update(dt);
            Turret.Update(dt);
            Track.Update(dt);
        }

        public bool DoesTankHaveEnoughPower(float value)
        {
            if (CurrentPower < value)
                return false;
            return true;
        }

        public void UsePower(float amount)
        {
            CurrentPower -= amount;
        }

        public void AddPower(float amount)
        {
            CurrentPower += amount;
            if (CurrentPower > GetMaxPower())
                CurrentPower = GetMaxPower();
        }

        public float GetMaxPower()
        {
            float maxPower = 0;
            maxPower += Hull.StoredPower;
            maxPower += Turret.StoredPower;
            maxPower += Track.StoredPower;

            foreach (Plugin p in Turret.Plugins)
            {
                maxPower += p.StoredPower;
            }

            return maxPower;
        }

        public float GetMaxHp()
        {
            float maxHp = 0;
            maxHp += Hull.StoredHp;
            maxHp += Turret.StoredHp;
            maxHp += Track.StoredHp;

            foreach (Plugin p in Turret.Plugins)
            {
                maxHp += p.StoredHp;
            }

            return maxHp;
        }

        public void Draw(IRender render, Camera camera)
        {
            Hull.Draw(render, camera);
            Turret.Draw(render, camera);
        }
    }
}
