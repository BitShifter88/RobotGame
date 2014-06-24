using Lidgren.Network;
using Macalania.Probototaker.Network;
using Macalania.Robototaker.Log;
using Macalania.YunaEngine.Resources;
using Macalania.YunaEngine.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Macalania.Robototaker.GameServer
{
    class ServerGame : GameLoop
    {

        ResourceManager _content;
        ServerRoom _room;


        public ServerGame(ResourceManager content, NetServer server)
        {
            _content = content;
            RoomManager rm = new RoomManager();
            
            _room = new ServerRoom(server);
            rm.SetActiveRoom(_room, true, content);
            
            StartGameLoop();
        }


        protected override void Update(double dt)
        {

            _room.Update(dt);

            base.Update(dt);
        }

        
    }
}
