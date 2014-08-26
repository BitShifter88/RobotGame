using Lidgren.Network;
using Macalania.Robototaker.Log;
using Macalania.Robototaker.MainFrame.Data.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Robototaker.MainFrame.Network.GameMainFrame
{
    enum SessionStatus
    {
        SignedIn,
        SignedOut,
    }

    class PlayerSession
    {
        public int MyProperty { get; set; }
        public int SessionId { get; set; }
        public Account Account { get; set; }
        public NetConnection Connection { get; set; }
        public SessionStatus Status { get; set; }

        public PlayerSession(NetConnection connection, Account account)
        {
            Status = SessionStatus.SignedIn;
            Connection = connection;
            Account = account;
            SessionId = SessionIdGenerator.GetNextId();
        }

        public void SignOut(string reason)
        {
            Status = SessionStatus.SignedOut;
            ServerLog.E("Player " + Account.Username + " signed out. Reason: " + reason, LogType.Security);
            Connection.Disconnect(reason);
        }
    }
}
