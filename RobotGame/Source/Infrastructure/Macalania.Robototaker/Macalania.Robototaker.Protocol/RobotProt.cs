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
    }
}
