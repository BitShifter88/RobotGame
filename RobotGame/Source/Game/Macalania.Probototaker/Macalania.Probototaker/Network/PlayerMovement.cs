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
        public RotationDirection RotationDir;

        public override bool Equals(object obj)
        {
            PlayerMovement pm = (PlayerMovement)obj;
            if (pm.DrivingDir == DrivingDir && pm.RotationDir == RotationDir)
                return true;
            return false;
        }
    }
}
