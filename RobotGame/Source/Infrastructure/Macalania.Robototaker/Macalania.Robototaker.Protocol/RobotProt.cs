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
        RequestFullWorldUpdate = 8,
        PlayerUsesAbility = 9,
    }
    public enum MainFrameProt : byte
    {
       CreatePlayer = 0,
        Login = 1,
        AskIfReadyForGame = 2,
    }
    public enum InfrastructureProt : byte
    {
        Authorize = 200,
        StartGameInstance = 201,
    }
}
