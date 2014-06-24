using Macalania.Probototaker.Tanks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Network
{
    public class PlayerMovement
    {
        public DrivingDirection DrivingDir;
        public RotationDirection BodyDir;
        public RotationDirection TurretDir;
        public bool MainGunFirering;
        public float TurretRotation;

        public override bool Equals(object obj)
        {
            PlayerMovement pm = (PlayerMovement)obj;
            if (pm.DrivingDir == DrivingDir && pm.BodyDir == BodyDir && TurretDir == pm.TurretDir && MainGunFirering == pm.MainGunFirering)
                return true;
            return false;
        }
    }
}
