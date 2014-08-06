using Macalania.Probototaker.Tanks.Hulls;
using Macalania.Probototaker.Tanks.Turrets;
using Macalania.Probototaker.Tanks.Plugins;
using Macalania.Probototaker.Tanks.Plugins.Mic;
using Macalania.Probototaker.Tanks.Tracks;
using Macalania.YunaEngine.GameLogic;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Rendering;
using Macalania.YunaEngine.Resources;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    public struct TankLogEntry
    {
        public Vector2 BodyDirection;
        public float CurrentSpeed;
        public float CurrentRotationSpeed;
        public Vector2 Position;
        public float BodyRotation;
        public double Dt;
    }

    public class Tank : GameObject
    {
        public byte ServerSideTankId { get; set; }

        public Hull Hull { get; private set; }
        public Track Track { get; private set; }
        //public Turret Turret { get; private set; }
        public Turret Turret { get; set; }
        public float BodyRotation { get; set; }
        public float TurretRotation { get; set; }
        public float CurrentPower { get; set; }
        public float CurrentHp { get; set; }
        public bool Dead { get; set; }
        public bool SheeldEnabled { get; set; }
        public bool ArtilleryFirering { get; set; }
        public DrivingDirection DrivingDir { get; set; }
        public RotationDirection BodyDir { get; set; }
        public RotationDirection TurretDir { get; set; }
        public bool MainGunFirering { get; set; }
        public float CurrentSpeed { get; set; }
        public float RotationAcceleration { get; set; }
        public float CurrentRotationSpeed { get; set; }

        public VisualTurretStyle TurretStyle { get; set; }

        public float TurretRotationSpeed { get; set; }

        // Used for player latency compensation
        bool _serverPlayerCompensation = false;
        public Vector2 LastKnownServerTankPosition { get; set; }
        public float LastKnownServerTankBodyRotation { get; set; }
        // Used for other player latency compensation

        bool _otherClientControled = false;
        public Vector2 EstimatedClientPosition { get; set; }
        public float EstimatedClientBodyRotation { get; set; }
        public float EstimatedClientBodyRotationSpeed { get; set; }
        public float EstimatedClientBodySpeed { get; set; }
        public float EstimatedClientTurretRotation { get; set; }

        private List<TankLogEntry> TankLog = new List<TankLogEntry>();


        public long Id { get; set; }


        // Attributes

        public float AmorPoints { get; set; }
        public float PowerRegen { get; set; }
        public float MaxSpeed { get; set; }
        public float MaxHp { get; set; }
        public float MaxPower { get; set; }
        public float MaxRotationSpeed { get; set; }
        public float Acceleration { get; set; }
        public float Weight { get; set; }
        public float HorsePower { get; set; }
        public float HullBearing { get; set; }

        public Tank(Vector2 position)
            : base(null)
        {
            SetPosition(position);
           
        }

        public void DamageTank(float amount, float amorPenetration)
        {
            CurrentHp -= amount;
            CheckIfDead();
        }

        public void SetServerEstimation(Vector2 position, float bodyRotation, float bodySpeed, float bodyRotationSpeed, float turretRotation)
        {
            EstimatedClientPosition = position;
            EstimatedClientBodyRotation = bodyRotation;
            EstimatedClientBodySpeed = bodySpeed;
            EstimatedClientBodyRotationSpeed = bodyRotationSpeed;
            EstimatedClientTurretRotation = turretRotation;

            _otherClientControled = true;
        }

        public void SetLastKnownServerInfo(Vector2 position, float bodyRotation, int latency)
        {
            TurnTimeForwardForOldPositionAndBodyRotation(ref position, ref bodyRotation, (float)latency * 2);

            //Console.WriteLine(Vector2.Distance(position, Position));

            LastKnownServerTankPosition = position; // MovePosition(position, (float)latency * 2);
            LastKnownServerTankBodyRotation = bodyRotation; // DoRotation(bodyRotation, latency);
            _serverPlayerCompensation = true;
        }

        public void TurnTimeForwardForOldPositionAndBodyRotation(ref Vector2 position, ref float bodyRotation, float ageOfPosition)
        {
            // Arbitrært... Vi vil ikke skrue tiden længere tilbage end hvad der er recorded
            if (TankLog.Count < ageOfPosition /7f || TankLog.Count < 120 || ageOfPosition == 0)
                return;

            double time = 0;
            for (int i = TankLog.Count - 1; i >= 0; i--)
            {
                time += TankLog[i].Dt;

                // TODO: Lav noget average ( < 0.5 >) her for at forbedre latency compensation
                if (time >= ageOfPosition)
                {
                    for (int j = i + 1; j < TankLog.Count - 1; j++)
                    {
                        position += TankLog[j].CurrentSpeed * TankLog[j].BodyDirection * (float)TankLog[j].Dt;
                        bodyRotation += TankLog[j].CurrentRotationSpeed * (float)TankLog[j].Dt;
                    }

                    break;
                }
            }
        }

        public void TurnTimeBack(double dt)
        {
            double time = 0;
            for (int i = TankLog.Count - 1; i >= 0; i--)
            {
                time += TankLog[i].Dt;

                if (time >= dt)
                {
                    SetPosition(TankLog[i].Position);
                    BodyRotation = TankLog[i].BodyRotation;
                    CurrentRotationSpeed = TankLog[i].CurrentRotationSpeed;
                    CurrentSpeed =  TankLog[i].CurrentSpeed;

                    // TODO: Det her range findes måske ikke
                    TankLog.RemoveRange(i + 1, TankLog.Count);
                    break;
                }
            }
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

        public void ReadyTank(Room room)
        {
            foreach (TurretModule p in Turret.GetModules())
            {
                p.Ready();
            }

            DrivingDir = DrivingDirection.Still;
            BodyDir = RotationDirection.Still;

            CalculateAttributes();

            Room = room;
        }

        private void CalculateAttributes()
        {
            MaxPower = GetMaxPower();
            CurrentPower = MaxPower;

            CurrentHp = GetMaxHp();

            MaxHp = GetMaxHp();
            CurrentHp = MaxHp;

            MaxSpeed = 0.15f;
            Acceleration = 0.0001f;

            MaxRotationSpeed = 0.0015f;
            RotationAcceleration = 0.000004f;

            TurretRotationSpeed = 0.0006f;

            AmorPoints = 0;
            PowerRegen = 0;

            foreach (TurretModule p in Turret.GetModules())
            {
                AmorPoints += p.AmorPoints;
                PowerRegen += p.PowerRegen;
            }
        }


        public TankComponent IsColliding(Sprite s)
        {
            foreach (TurretModule p in Turret.GetModules())
            {
                if (p.CheckCollision(s))
                    return p;
            }

            if (Hull.CheckCollision(s))
                return Hull;
            //if (Turret.CheckCollision(s))
            //    return Turret;

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

        public  Vector2 GetTurretBodyDirection()
        {
            Vector2 dir = new Vector2((float)Math.Cos((double)TurretRotation + BodyRotation + MathHelper.ToRadians(90)), (float)Math.Sin((double)TurretRotation + BodyRotation + MathHelper.ToRadians(90)));
            dir.Normalize();
            return dir;
        }

        public float GetTurrentBodyRotation()
        {
            return TurretRotation + BodyRotation + MathHelper.ToRadians(180);
        }

        public void SetHull(Hull hull)
        {
            Hull = hull;
            Hull.Sprite.Position = Position;
        }

        public void SetTrack(Track track)
        {
            Track = track;
            Track.Sprite.Position = Position;
        }

        public void SetTurret(Turret turret)
        {
            Turret = turret;
        }

        public void FireMainGun()
        {
            MainGunFirering = true;
        }

        public void StopFireMainGun()
        {
            MainGunFirering = false;
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

            Vector2 turretRotation = GetTurretBodyDirection();
            Vector2 pointDir = Position - point;
            pointDir.Normalize();

            float angle = (float)Math.Acos((float)Vector2.Dot(pointDir, turretRotation));

            Vector2 turretRotationAfterExtra = new Vector2((float)Math.Cos((double)TurretRotation + 0.01f + BodyRotation + MathHelper.ToRadians(90)), (float)Math.Sin((double)TurretRotation + 0.01f + BodyRotation + MathHelper.ToRadians(90)));
            float angleAfterExtra = (float)Math.Acos((float)Vector2.Dot(pointDir, turretRotationAfterExtra));

            if (angleAfterExtra > angle)
            {
                if (angle > 0.005f)
                {
                    TurretDir = RotationDirection.ClockWise;
                }
                else
                    TurretDir = RotationDirection.Still;
            }
            else
            {
                if (angle > 0.005f)
                {
                    TurretDir = RotationDirection.CounterClockWise;
                }
                else
                {
                    TurretDir = RotationDirection.Still;
                }
            }
        }

        private float AccelerateTank(double dt, float currentSpeed)
        {
            if (DrivingDir == DrivingDirection.Forward)
            {
                if (CurrentSpeed < 0)
                {
                    currentSpeed += Acceleration * 2 * (float)dt;
                }
                else
                {
                    currentSpeed += Acceleration * (float)dt;
                }
            }
            if (DrivingDir == DrivingDirection.Backwards)
            {
                if (currentSpeed > 0)
                {
                    currentSpeed -= Acceleration * 2 * (float)dt;
                }
                else
                {
                    currentSpeed -= Acceleration * (float)dt;
                }
            }
            if (DrivingDir == DrivingDirection.Still)
            {
                float beforeSpeed = currentSpeed;
                if (currentSpeed > 0)
                    currentSpeed -= Acceleration * 2 * (float)dt;
                else if (CurrentSpeed < 0)
                    currentSpeed += Acceleration * 2 * (float)dt;

                if ((beforeSpeed > 0 && currentSpeed < 0) || (beforeSpeed < 0 && currentSpeed > 0))
                    currentSpeed = 0;
            }

            if (currentSpeed > MaxSpeed)
                currentSpeed = MaxSpeed;
            if (currentSpeed < -MaxSpeed)
                currentSpeed = -MaxSpeed;

            return currentSpeed;
        }

        public Vector2 MovePosition(Vector2 pos, float currentSpeed, float dt)
        {
            pos = pos + currentSpeed * GetBodyDirection() * dt;
            return pos;
        }
       
        private float AccelerateRotateTank(double dt, float currentRotationSpeed)
        {
            if (BodyDir == RotationDirection.ClockWise)
            {
                currentRotationSpeed += RotationAcceleration * (float)dt;
            }
            if (BodyDir == RotationDirection.CounterClockWise)
            {
                currentRotationSpeed -= RotationAcceleration * (float)dt;
            }
            if (BodyDir == RotationDirection.Still)
            {
                float beforeSpeed = currentRotationSpeed;
                if (currentRotationSpeed > 0)
                    currentRotationSpeed -= RotationAcceleration * (float)dt;
                else if (CurrentRotationSpeed < 0)
                    currentRotationSpeed += RotationAcceleration * (float)dt;

                if ((beforeSpeed > 0 && currentRotationSpeed < 0) || (beforeSpeed < 0 && currentRotationSpeed > 0))
                    currentRotationSpeed = 0;
            }

            if (currentRotationSpeed > MaxRotationSpeed)
                currentRotationSpeed = MaxRotationSpeed;
            if (currentRotationSpeed < -MaxRotationSpeed)
                currentRotationSpeed = -MaxRotationSpeed;

            return currentRotationSpeed;
        }

        public float DoBodyRotation(float rotation, float currentRotationSpeed, float dt)
        {
            return rotation + currentRotationSpeed * (float)dt;
        }

        public void RotateBody(RotationDirection dir)
        {
            if (ArtilleryFirering)
                return;

            BodyDir = dir;
        }
        private void SmoothOtherClient(double dt)
        {
            if (Vector2.Distance(EstimatedClientPosition, Position) > 0.1f && CurrentSpeed > MaxSpeed / 10f)
            {
                //Vector2 smooti = new Vector2((Position.X - EstimatedClientPosition.X) * 0.01f, (Position.Y - EstimatedClientPosition.Y) * 0.01f);

                SetPosition(new Vector2(Position.X - (Position.X - EstimatedClientPosition.X) * 0.1f, Position.Y - (Position.Y - EstimatedClientPosition.Y) * 0.1f));
            }
            if (Math.Abs((BodyRotation - EstimatedClientBodyRotation)) > 0.000003f)
            {
                BodyRotation = BodyRotation - ((BodyRotation - EstimatedClientBodyRotation) * 0.1f);
                //Console.WriteLine("Smoothing rotation");
            }

            if (Math.Abs(TurretRotation - EstimatedClientTurretRotation) > 0.000003f)
            {
                TurretRotation = TurretRotation - ((TurretRotation - EstimatedClientTurretRotation) * 0.1f);
            }
        }
        private void SmoothPlayerCompensation(double dt)
        {
            if (_serverPlayerCompensation == false)
                return;
            if (CurrentSpeed < MaxSpeed / 10f)
                return;

            if (Vector2.Distance(LastKnownServerTankPosition, Position) > 0.1f)
            {
                //Vector2 smooti = new Vector2((Position.X - LastKnownServerTankPosition.X) * 0.1f,(Position.Y - LastKnownServerTankPosition.Y) * 0.1f);

                SetPosition(new Vector2(Position.X - (Position.X - LastKnownServerTankPosition.X) * 0.01f, Position.Y - (Position.Y - LastKnownServerTankPosition.Y) * 0.01f));
            }
            if (Math.Abs((BodyRotation - LastKnownServerTankBodyRotation)) > 0.000003f)
            {
                BodyRotation = BodyRotation - ((BodyRotation - LastKnownServerTankBodyRotation) * 0.01f);
                //Console.WriteLine("Smoothing rotation");
            }
        }

        public void UpdateServerPositionRotation(double dt)
        {
            LastKnownServerTankPosition = MovePosition(LastKnownServerTankPosition, CurrentSpeed, (float)dt);
            LastKnownServerTankBodyRotation = DoBodyRotation(LastKnownServerTankBodyRotation, CurrentRotationSpeed, (float)dt);
        }

        public void UpdateServerEstimation(double dt)
        {
            EstimatedClientBodySpeed = AccelerateTank(dt, EstimatedClientBodySpeed);
            EstimatedClientBodyRotationSpeed = AccelerateRotateTank(dt, EstimatedClientBodyRotationSpeed);

            EstimatedClientPosition = MovePosition(EstimatedClientPosition, EstimatedClientBodySpeed, (float)dt);
            EstimatedClientBodyRotation = DoBodyRotation(EstimatedClientBodyRotation, EstimatedClientBodyRotationSpeed, (float)dt);

            EstimatedClientTurretRotation = RotateTurret(dt, EstimatedClientTurretRotation);
        }

        private float RotateTurret(double dt, float turretRotation)
        {
            if (TurretDir == RotationDirection.CounterClockWise)
                turretRotation += TurretRotationSpeed * (float)dt;
            if (TurretDir == RotationDirection.ClockWise)
                turretRotation -= TurretRotationSpeed * (float)dt;

            return turretRotation;
        }

        private void UpdateMainGun()
        {
            if (ArtilleryFirering)
                return;
            if (SheeldEnabled)
                return;

            if (MainGunFirering)
                Turret.FireMainGun();
        }

        public override void Update(double dt)
        {
            CurrentSpeed = AccelerateTank(dt, CurrentSpeed);
            SetPosition(MovePosition(Position, CurrentSpeed, (float)dt));
                
            CurrentRotationSpeed = AccelerateRotateTank(dt, CurrentRotationSpeed);
            BodyRotation = DoBodyRotation(BodyRotation, CurrentRotationSpeed, (float)dt);

            TurretRotation = RotateTurret(dt, TurretRotation);

            if (_serverPlayerCompensation)
            {
                UpdateServerPositionRotation(dt);
                SmoothPlayerCompensation(dt);
            }

            if (_otherClientControled)
            {
                UpdateServerEstimation(dt);
                SmoothOtherClient(dt);
            }

            
            Hull.Update(dt);
            Turret.Update(dt);
            Track.Update(dt);

            UpdateMainGun();

            UpdatePowerRegen(dt);

            LogTank(dt);
        }

        private void UpdatePowerRegen(double dt)
        {
            AddPower((float)dt * PowerRegen / 100f);
        }

        private void LogTank(double dt)
        {
            TankLog.Add(new TankLogEntry() { BodyDirection = GetBodyDirection(), BodyRotation = BodyRotation, CurrentRotationSpeed = CurrentRotationSpeed, CurrentSpeed = CurrentSpeed, Position = Position, Dt = dt });

            if (TankLog.Count > 60 * 5)
            {
                TankLog.RemoveAt(0);
            }
        }

        public override void Load(ResourceManager content)
        {
            TurretStyle = new ClasicStyle(content);

            //TurretNew.AddTurretComponent(new TurretBrick(this), 32-2, 30);

            


            base.Load(content);
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

        public bool ActivatePlugin(PluginType type, Vector2 targetPosition, Tank targetTank)
        {
            List<TurretModule> pluginsOfType = new List<TurretModule>();

            foreach (TurretModule p in Turret.GetModules())
            {
                if (p.PluginType == type)
                    pluginsOfType.Add(p);
            }

            foreach (TurretModule p in pluginsOfType)
            {
                if (p.Activate(targetPosition, targetTank) == false)
                    continue;
                else
                    return true;
            }

            return false;
        }

        //public void ActivatePlugin(Plugin p, Vector2 targetPosition, Tank targetTank)
        //{
        //    if (ArtilleryFirering)
        //        return;
        //    if (SheeldEnabled)
        //        return;
        //    Turret.ActivatePlugin(p, targetPosition, targetTank);
        //}

        public float GetMaxPower()
        {
            float maxPower = 0;
            maxPower += Hull.StoredPower;
            maxPower += Track.StoredPower;

            foreach (TurretModule p in Turret.GetModules())
            {
                maxPower += p.StoredPower;
            }


            return maxPower;
        }

        public float GetMaxHp()
        {
            float maxHp = 0;
            maxHp += Hull.StoredHp;
            maxHp += Track.StoredHp;

            foreach (TurretModule p in Turret.GetModules())
            {
                maxHp += p.StoredHp;
            }

            return maxHp;
        }

        public override void Draw(IRender render, Camera camera)
        {
            Hull.Draw(render, camera);
            Turret.Draw(render, camera);

            //Turret.Draw(render, camera);
        }
    }
}
