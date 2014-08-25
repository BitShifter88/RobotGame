using Lidgren.Network;
using Macalania.Robototaker.MainFrame.Data.Mapping;
using Macalania.Robototaker.MainFrame.Services;
using Macalania.Robototaker.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Robototaker.MainFrame.Network.GameMainFrame
{
    class SessionManager
    {
        private AccountService _as = new AccountService();
        private NetServer _server;
        private Dictionary<int, PlayerSession> _playerSessions = new Dictionary<int, PlayerSession>();

        public SessionManager(NetServer server)
        {
            _server = server;
        }

        public void LoginAttempt(NetConnection connection, string username, string password)
        {
            Account account = _as.IsValidLogin(username, password);
            if (account != null)
            {
                PlayerSession ps = CreatePlayerSession(connection, account);
                LoginAttemptResponse(true, ps.SessionId, connection);
            }
            else
                LoginAttemptResponse(false, 0, connection);
        }

        private PlayerSession CreatePlayerSession(NetConnection connection, Account account)
        {
            for (int i = 0; i < _playerSessions.Values.Count; i++)
            {
                if (_playerSessions.Values.ElementAt(i).Account.Username == account.Username)
                {
                    SignOutPlayerSession(_playerSessions.Values.ElementAt(i));
                    break;
                }
            }

            PlayerSession ps = new PlayerSession(connection, account);
            _playerSessions.Add(ps.SessionId, ps);

            return ps;
        }

        public void DisconnectedPlayer(NetConnection connection)
        {
            for (int i = 0; i < _playerSessions.Values.Count; i++)
            {
                if (_playerSessions.Values.ElementAt(i).Connection.RemoteUniqueIdentifier == connection.RemoteUniqueIdentifier)
                {
                    _playerSessions.Remove(_playerSessions.Values.ElementAt(i).SessionId);
                    break;
                }
            }
        }

        private void SignOutPlayerSession(PlayerSession session)
        {
            _playerSessions.Remove(session.SessionId);
            session.SignOut();
        }

        private void LoginAttemptResponse(bool success, int sessionId, NetConnection connection)
        {
            NetOutgoingMessage m = _server.CreateMessage();
            m.Write((byte)MainFrameProt.Login);
            m.Write(success);
            if (success)
                m.Write(sessionId);
            connection.SendMessage(m, NetDeliveryMethod.ReliableUnordered, 0);
        }
    }
}
