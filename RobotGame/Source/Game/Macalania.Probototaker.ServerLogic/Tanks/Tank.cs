using Macalania.Probototaker.Tanks.Hulls;
using Macalania.Probototaker.Tanks.Plugins;
using Macalania.Probototaker.Tanks.Tracks;
using Macalania.Probototaker.Tanks.Turrets;
using Macalania.YunaEngine.GameLogic;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Rendering;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Macalania.Probototaker.Tanks
{
    public enum RotationDirection : byte
    {
        Still = 0,
        CounterClockWise = 1,
        ClockWise = 2,
    }

    public enum DrivingDirection : byte
    {
        Still = 0,
        Forward = 1,
        Backwards = 2,
    }

    public class Tank : GameObject
    {
        public Vector2 Position { get; private set; }
        public Hull Hull { get; private set; }
        public Track Track { get; private set; }
        public Turret Turret { get; private set; }
        public float BodyRotation { get; set; }
        public float TurretRotation { get; set; }
        public float CurrentPower { get; set; }
        public float CurrentHp { get; set; }
        public float MaxHp { get; set; }
        public bool Dead { get; set; }
        public bool SheeldEnabled { get; set; }
        public bool ArtilleryFirering { get; set; }
        public DrivingDirection DrivingDir { get; set; }
        public RotationDirection RotationDir { get; set; }
        public float CurrentSpeed { get; set; }
        public float MaxSpeed { get; set; }
        public float Acceleration { get; set; }
        public float RotationAcceleration { get; set; }
        public float CurrentRotationSpeed { get; set; }
        public float MaxRotationSpeed { get; set; }

        //public BoundingSphere BoundingSphere { get; set; }

        public Tank(Room room, Vector2 position)
            : base(room)
        {
            Position = position;
           
        }

        public void DamageTank(float amount, float amorPenetration)
        {
            CurrentHp -= amount;
            CheckIfDead();
        }

        public bool IsStandingStill()
        {
            if (CurrentRotationSpeed == 0 && CurrentSpeed == 0)
                return true;
            return false;
        }

        private void CheckIfDead()
        {
            if (Dead)
                return;
            if (CurrentHp <= 0)
                OnDead();
        }

        public void OnDead()
        {
            Dead = true;
            Turret.OnTankDestroy();
            Hull.OnTankDestroy();

            DestroyGameObject();
        }

        public void ReadyTank()
        {
            CurrentPower = GetMaxPower();
            CurrentHp = GetMaxHp();

            foreach (Plugin p in Turret.Plugins)
            {
                p.Ready();
            }

            MaxHp = GetMaxHp();
            CurrentHp = MaxHp;

            DrivingDir = DrivingDirection.Still;

            MaxSpeed = 0.15f;
            Acceleration = 0.0001f;

            MaxRotationSpeed = 0.0015f;
            RotationAcceleration = 0.000005f;
            //CalculateBoundingSphere();
        }

        public TankComponent IsColliding(Sprite s)
        {

            foreach (Plugin p in Turret.Plugins)
            {
                if (p.CheckCollision(s))
                    return p;
            }

            if (Hull.CheckCollision(s))
                return Hull;
            if (Turret.CheckCollision(s))
                return Turret;


            return null;
        }

        //private void CalculateBoundingSphere()
        //{
        //    BoundingSphere b = BoundingSphere.CreateMerged(Turret.Sprite.RelativeBoundingSphere, Hull.Sprite.RelativeBoundingSphere);

        //    BoundingSphere = b;
        //}

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
            if (ArtilleryFirering)
                return;
            if (SheeldEnabled)
                return;
            Turret.FireMainGun();
        }

        public void Thruttle(DrivingDirection dir)
        {
            if (ArtilleryFirering)
                return;
            DrivingDir = dir;
        }

        int counter = 0;
        public void MoveTurretTowardsPoint(Vector2 point)
        {
            if (ArtilleryFirering)
                return;

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



        private void MoveTank(double dt)
        {
            if (DrivingDir == DrivingDirection.Forward)
            {
                if (CurrentSpeed < 0)
                {
                    CurrentSpeed += Acceleration * 2 * (float)dt;
                }
                else
                {
                    CurrentSpeed += Acceleration * (float)dt;
                }
            }
            if (DrivingDir == DrivingDirection.Backwards)
            {
                if (CurrentSpeed > 0)
                {
                    CurrentSpeed -= Acceleration * 2 * (float)dt;
                }
                else
                {
                    CurrentSpeed -= Acceleration * (float)dt;
                }
            }
            if (DrivingDir == DrivingDirection.Still)
            {
                float beforeSpeed = CurrentSpeed;
                if (CurrentSpeed > 0)
                    CurrentSpeed -= Acceleration * 2 * (float)dt;
                else if (CurrentSpeed < 0)
                    CurrentSpeed += Acceleration * 2 * (float)dt;

                if ((beforeSpeed > 0 && CurrentSpeed < 0) || (beforeSpeed < 0 && CurrentSpeed > 0))
                    CurrentSpeed = 0;
            }

            if (CurrentSpeed > MaxSpeed)
                CurrentSpeed = MaxSpeed;
            if (CurrentSpeed < -MaxSpeed)
                CurrentSpeed = -MaxSpeed;

            Position += CurrentSpeed * GetBodyDirection() * (float)dt;
        }

        private void RotateTank(double dt)
        {
            if (dt < 0)
            {
                BodyRotation += CurrentRotationSpeed * (float)dt;
            }
            if (RotationDir == RotationDirection.ClockWise)
            {
                CurrentRotationSpeed += RotationAcceleration * (float)dt;
            }
            if (RotationDir == RotationDirection.CounterClockWise)
            {
                CurrentRotationSpeed -= RotationAcceleration * (float)dt;
            }
            if (RotationDir == RotationDirection.Still)
            {
                float beforeSpeed = CurrentRotationSpeed;
                if (CurrentRotationSpeed > 0)
                    CurrentRotationSpeed -= RotationAcceleration * (float)dt;
                else if (CurrentRotationSpeed < 0)
                    CurrentRotationSpeed += RotationAcceleration * (float)dt;

                if ((beforeSpeed > 0 && CurrentRotationSpeed < 0) || (beforeSpeed < 0 && CurrentRotationSpeed > 0))
                    CurrentRotationSpeed = 0;
            }

            if (CurrentRotationSpeed > MaxRotationSpeed)
                CurrentRotationSpeed = MaxRotationSpeed;
            if (CurrentRotationSpeed < -MaxRotationSpeed)
                CurrentRotationSpeed = -MaxRotationSpeed;

            if (dt > 0)
            BodyRotation += CurrentRotationSpeed * (float)dt;
        }

        public void RotateBody(RotationDirection dir)
        {
            if (ArtilleryFirering)
                return;

            RotationDir = dir;
        }

        public override void Update(double dt)
        {
            MoveTank(dt);
            RotateTank(dt);
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

        public void ActivatePlugin(Plugin p, Vector2 targetPosition, Tank targetTank)
        {
            if (ArtilleryFirering)
                return;
            if (SheeldEnabled)
                return;
            Turret.ActivatePlugin(p, targetPosition, targetTank);
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

        public override void Draw(IRender render, Camera camera)
        {
            Hull.Draw(render, camera);
            Turret.Draw(render, camera);
        }
    }
}
