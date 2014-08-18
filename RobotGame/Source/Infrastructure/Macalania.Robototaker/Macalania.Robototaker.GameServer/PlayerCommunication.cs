using Macalania.Probototaker.Tanks.Plugins;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Macalania.Robototaker.Protocol;

namespace Macalania.Robototaker.GameServer
{
    class PlayerCommunication
    {
        public static void BroadcastAbilityActivationToPlayers(NetServer server, GamePlayer player, List<GamePlayer> gamePlayers, PluginType type, byte targetTank, Vector2 targetPosition, ushort activationSeed)
        {
            foreach (GamePlayer p in gamePlayers)
            {
                NetOutgoingMessage m = server.CreateMessage() ;
                m.Write((byte)RobotProt.PlayerUsesAbility);
                m.Write(player.TankId);
                m.Write((byte)type);
                m.Write(targetTank);
                m.Write(targetPosition.X);
                m.Write(targetPosition.Y);
                m.Write(activationSeed);
                p.Connection.SendMessage(m, NetDeliveryMethod.ReliableUnordered, 0);
            }
        }
    }
}
