using Macalania.Probototaker.Network;
using Macalania.Robototaker.Protocol;
using Macalania.YunaEngine;
using Macalania.YunaEngine.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
            if (_connection.Connect(new MainFrameMessageHandler(_connection)) == false)
            {
                MessageBox.Show("Could not connect to Main Frame!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                YunaGameEngine.Instance.Exit();
            }
                    
            _connection.CreateAccount("steffan88", "MileyCyrus", "kodekode");
            _connection.Login("steffan88", "kodekode");
            LoginStatus ls= _connection.WaitForLoginResponse(5000);
            Console.WriteLine(ls);

            if (ls == LoginStatus.LoggedIn)
            {
                Room room = new Garage();
                YunaGameEngine.Instance.SetActiveRoom(room, true);
            }
#endif
        }
    }
}
