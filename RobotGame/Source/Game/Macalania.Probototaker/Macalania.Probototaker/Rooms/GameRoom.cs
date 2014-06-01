using Macalania.YunaEngine;
using Macalania.YunaEngine.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Rooms
{
    class GameRoom : Room
    {
        public GameRoom(YunaGameEngine engine) : base(engine)
        {

        }

        public override void Inizialize()
        {
            Player player = new Player();
            AddGameObject(player);

            base.Inizialize();
        }
    }
}
