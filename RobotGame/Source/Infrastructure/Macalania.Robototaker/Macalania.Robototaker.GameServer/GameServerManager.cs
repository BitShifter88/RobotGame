using Lidgren.Network;
using Macalania.YunaEngine.Resources;
using Macalania.YunaEngine.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Robototaker.GameServer
{
    class GameServerManager : GameLoop
    {
        Dictionary<long, GameInstance> _instances = new Dictionary<long, GameInstance>();
        long _idCounter = 0;

        ResourceManager _content;
        ServerRoom _room;


        public GameServerManager(ResourceManager content, NetServer server)
        {
            _content = content;
            
            _room = new ServerRoom(server);
            
            StartGameLoop();
        }


        protected override void Update(double dt)
        {
            _room.Update(dt);

            base.Update(dt);
        }


        //public GameServerInstance CreateNewGameServerInstance()
        //{
            
        //}

        private long GetNextId()
        {
            return _idCounter;
            _idCounter++;
        }
    }
}
