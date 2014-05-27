using Macalania.Probototaker.Tanks.Hulls;
using Macalania.Probototaker.Tanks.Tracks;
using Macalania.Probototaker.Tanks.Turrets;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public Tank()
        {
            Position = new Vector2(200, 200);
        }

        private Vector2 GetBodyDirection()
        {
            return new Vector2((float)Math.Cos((double)BodyRotation - MathHelper.ToRadians(90)), (float)Math.Sin((double)BodyRotation - MathHelper.ToRadians(90)));
        }

        private Vector2 GetTurretDirection()
        {
            return new Vector2((float)Math.Cos((double)TurretRotation), (float)Math.Sin((double)TurretRotation));
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

        public void Forward()
        {
            Position += GetBodyDirection();
        }

        public void Backwards()
        {
            Position -= GetBodyDirection();
        }

        public void MoveTurretTowardsPoint(Vector2 point)
        {
            Vector2 pointDir = Position - point;
            pointDir.Normalize();
            TurretRotation += 0.01f;
        }

        public void RotateBody(RotationDirection dir)
        {
            if (dir == RotationDirection.ClockWise)
                BodyRotation += 0.1f;
            else
                BodyRotation -= 0.1f;
        }
        public void Update(double dt)
        {
            Hull.Update(dt);
            Turret.Update(dt);
            Track.Update(dt);
        }

        public void Draw(IRender render, Camera camera)
        {
            Hull.Draw(render, camera);
            Turret.Draw(render, camera);
        }
    }
}
