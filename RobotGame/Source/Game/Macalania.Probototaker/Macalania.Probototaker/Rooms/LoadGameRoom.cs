using Macalania.Probototaker.Network;
using Macalania.Probototaker.Tanks;
using Macalania.Robototaker;
using Macalania.Robototaker.Protocol;
using Macalania.YunaEngine;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Rendering;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
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

        Animation _loadingAnimation;
        Sprite _background;

        GameRoom _gameRoom;
        GameNetwork _gameNetwork;
        TankPackage _tp;

        MainFrameConnection _connection;

        private bool _loadingDone = false;
        private bool _connectingDone = false;
        private bool _connectionFailed = false;

        public LoadGameRoom(TankPackage tp, MainFrameConnection connection)
        {
            _connection = connection;
            _tp = tp;
        }

        public override void Load(IServiceProvider serviceProvider)
        {
            base.Load(serviceProvider);

            _background = new Sprite(Content.LoadYunaTexture("Textures/Misc/loading"));

            _gameNetwork = new GameNetwork();
            _gameRoom = new GameRoom(_gameNetwork, _tp);

            _loadThread = new Thread(LoadGameRoomThread);
            _loadThread.Priority = ThreadPriority.BelowNormal;
            _loadThread.Start();
        }

        private void LoadGameRoomThread()
        {
            _gameRoom.Inizialize();
            _gameRoom.Load(YunaGameEngine.Instance.Services);
            PreLoader.PreLoad(_gameRoom.Content);
            _loadingDone = true;

            if (_gameNetwork.Start(_gameRoom, _tp, _connection.GameServerIp, _connection.QuedGameId) == false)
            {
                _connectionFailed = true;
                _gameRoom.Unload();
                return;
            }
            _connectingDone = true;
        }

        public override void Update(double dt)
        {
            if (_connectingDone == true && _loadingDone == true)
                YunaGameEngine.Instance.SetActiveRoom(_gameRoom, false);
            if (_connectionFailed == true)
            {
                Garage g = new Garage(_connection);
                YunaGameEngine.Instance.SetActiveRoom(g, true);
            }

            base.Update(dt);
        }

        protected override void DrawOther(IRender render, Camera camera)
        {
            render.Draw(_background.Texture, new Rectangle(0,0,Globals.Viewport.Width, Globals.Viewport.Height), Color.White);
        }
    }
}
