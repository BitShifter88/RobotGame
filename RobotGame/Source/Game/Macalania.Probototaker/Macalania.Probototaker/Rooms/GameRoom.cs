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
    class GameRoom : Room
    {
        public GameRoom(YunaGameEngine engine) : base(engine)
        {

        }

        public List<Tank> GetTanks()
        {
            List<Tank> tanks = new List<Tank>();

            foreach (GameObject obj in GameObjects)
            {
                if (obj.GetType() == typeof(Tank))
                    tanks.Add((Tank)obj);
            }

            return tanks;
        }

        public override void Inizialize()
        {
            Player player = new Player(this);
            AddGameObject(player);

            OtherPlayer op = new OtherPlayer(this);
            AddGameObject(op);

            base.Inizialize();
        }
    }
}
