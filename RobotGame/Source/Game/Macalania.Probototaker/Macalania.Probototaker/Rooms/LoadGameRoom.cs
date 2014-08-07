using Macalania.Probototaker.Network;
using Macalania.Probototaker.Tanks;
using Macalania.YunaEngine;
using Macalania.YunaEngine.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Macalania.Probototaker.Rooms
{
    public class LoadGameRoom : Room
    {
        Thread _loadThread;

        GameRoom _gameRoom;
        GameNetwork _gameNetwork;
        TankPackage _tp;

        private bool _loadingDone = false;
        private bool _connectingDone = false;

        public LoadGameRoom(TankPackage tp)
        {
            _tp = tp;
        }

        public override void Load(IServiceProvider serviceProvider)
        {
            _gameNetwork = new GameNetwork();
            _gameRoom = new GameRoom(_gameNetwork, _tp);

            _loadThread = new Thread(LoadGameRoomThread);
            _loadThread.Priority = ThreadPriority.BelowNormal;
            _loadThread.Start();

            base.Load(serviceProvider);
        }



        private void LoadGameRoomThread()
        {
            _gameRoom.Inizialize();
            _gameRoom.Load(YunaGameEngine.Instance.Services);
            _loadingDone = true;

            _gameNetwork.Start(_gameRoom);
            _connectingDone = true;
        }

        public override void Update(double dt)
        {
            if (_connectingDone == true && _loadingDone == true)
                RoomManager.Instance.SetActiveRoom(_gameRoom, false, YunaGameEngine.Instance.Services);

            base.Update(dt);
        }

        public override void Draw(YunaEngine.Rendering.IRender render)
        {
            base.Draw(render);
        }
    }
}
