﻿using Macalania.Probototaker.Network;
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
        Thread _connectionThread;

        GameRoom _gameRoom;
        GameNetwork _gameNetwork;

        private bool _loadingDone = false;
        private bool _connectingDone = false;

        public LoadGameRoom()
        {
        }

        public override void Load(IServiceProvider serviceProvider)
        {
            _loadThread = new Thread(LoadGameRoomThread);
            _loadThread.Priority = ThreadPriority.BelowNormal;
            _loadThread.Start();

            _connectionThread = new Thread(ConnectThread);
            _connectionThread.Priority = ThreadPriority.BelowNormal;
            _connectionThread.Start();

            base.Load(serviceProvider);
        }

        private void ConnectThread()
        {
            _gameNetwork = new GameNetwork();
            _gameNetwork.Start();
            _connectingDone = true;
        }

        private void LoadGameRoomThread()
        {
            _gameRoom = new GameRoom();
            _gameRoom.Inizialize();
            _gameRoom.Load(YunaGameEngine.Instance.Services);
            _loadingDone = true;
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
