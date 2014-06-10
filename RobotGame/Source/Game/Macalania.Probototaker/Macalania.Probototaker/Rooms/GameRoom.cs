using Macalania.Probototaker.Effects;
using Macalania.Probototaker.Tanks;
using Macalania.YunaEngine;
using Macalania.YunaEngine.GameLogic;
using Macalania.YunaEngine.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Rooms
{
    public class GameRoom : SimulationRoom
    {
        public override void Inizialize()
        {
            Player player = new Player(this);
            AddGameObject(player);

            OtherPlayer op = new OtherPlayer(this, 1);
            AddGameObject(op);

            OtherPlayer op2 = new OtherPlayer(this, 2);
            AddGameObject(op2);

            base.Inizialize();
        }
    }
}
