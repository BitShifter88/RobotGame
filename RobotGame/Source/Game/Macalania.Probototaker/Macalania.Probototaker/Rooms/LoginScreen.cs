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
            string name = new Random().Next(0, 100000000).ToString();
            _connection.CreateAccount(name, "MileyCyrus", "kodekode");
            _connection.Login(name, "kodekode");
            LoginStatus ls = _connection.WaitForLogin(10000);
            Console.WriteLine(ls);

            if (ls == LoginStatus.LoggedIn)
            {
                Room room = new Garage(_connection);
                YunaGameEngine.Instance.SetActiveRoom(room, true);
            }
#endif
        }
    }
}
