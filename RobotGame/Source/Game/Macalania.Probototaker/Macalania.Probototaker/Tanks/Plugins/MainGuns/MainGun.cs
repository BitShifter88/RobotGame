using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Plugins.MainGuns
{
    class MainGun : Plugin
    {
        public override void Update(double dt)
        {
            base.Update(dt);

            Sprite.Rotation = Tank.TurretRotation + Tank.BodyRotation;
        }
    }
}
