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


        public GameServerManager()
        {
            _content = new ResourceManager(null);
                       
            StartGameLoop();
        }

        private void StartServer()
        {
            
        }

        protected override void Update(double dt)
        {
            foreach (KeyValuePair<long, GameInstance> instance in _instances)
            {
                instance.Value.Update(dt);
            }

            base.Update(dt);
        }


        public GameInstance CreateNewGameInstance()
        {
            GameInstance gi = new GameInstance();
            gi.StartGame(_content);
            _instances.Add(GetNextId(), gi);

            return gi;
        }

        private long GetNextId()
        {
            return _idCounter;
            _idCounter++;
        }
    }
}
