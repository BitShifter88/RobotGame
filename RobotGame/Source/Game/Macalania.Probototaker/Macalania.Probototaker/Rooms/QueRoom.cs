using Macalania.Probototaker.Tanks;
using Macalania.Robototaker.Protocol;
using Macalania.YunaEngine;
using Macalania.YunaEngine.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Rooms
{
    class QueRoom : Room
    {
        MainFrameConnection _connection;
        TankPackage _tank;

        public QueRoom(MainFrameConnection connection, TankPackage tankPackage)
        {
            _tank = tankPackage;
            _connection = connection;
        }

        public override void Load(IServiceProvider serviceProvider)
        {
            _connection.JoinQue();
            Console.WriteLine("Que joined");
            base.Load(serviceProvider);
        }

        public override void Update(double dt)
        {
            if (_connection.QueStatus == QueStatus.InGame)
            {
                Console.WriteLine("Game created");
                LoadGameRoom lgr = new LoadGameRoom(_tank, _connection);
                YunaGameEngine.Instance.SetActiveRoom(lgr, true);
            }

            base.Update(dt);
        }
    }
}
