using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macalania.Robototaker.Protocol
{
    public enum RobotProt : byte
    {
        PlayerIdentification = 0,
        PlayerMovement = 1,
        PlayerCompensation = 2,
        OtherPlayerInfoMovement = 3,
        ProjectileInfo = 4,
        AbilityActivation = 5,
        CreateOtherPlayer = 6,
        FullWorldUpdate = 7,
    }
}
