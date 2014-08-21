using Lidgren.Network;
using Macalania.Robototaker.MainFrame.Data.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Robototaker.MainFrame.Network.GameMainFrame
{
    class PlayerSession
    {
        public int SessionId { get; set; }
        public Account Account { get; set; }
        public NetConnection Connection { get; set; }

        public PlayerSession(NetConnection connection, Account account)
        {
            Connection = connection;
            Account = account;
            SessionId = SessionIdGenerator.GetNextId();
        }

        public void SignOut()
        {
            Connection.Disconnect("Another computer has signed into your account");
        }
    }
}
