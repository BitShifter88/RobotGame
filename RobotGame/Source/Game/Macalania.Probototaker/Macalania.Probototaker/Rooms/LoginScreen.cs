using Macalania.Probototaker.Network;
using Macalania.Robototaker.Protocol;
using Macalania.YunaEngine.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Rooms
{
    class LoginScreen : Room
    {
        MainFrameConnection _connection;

        public LoginScreen()
        {
            _connection = new MainFrameConnection();
        }

        public override void Load(IServiceProvider serviceProvider)
        {
            base.Load(serviceProvider);

#if DEBUG
            _connection.Connect(new MainFrameMessageHandler());
#endif
        }
    }
}
