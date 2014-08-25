using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Robototaker.Protocol
{
    public class MainFrameMessageHandlerBase
    {
        MainFrameConnection _connection;

        public MainFrameMessageHandlerBase(MainFrameConnection connection)
        {
            _connection = connection;
        }

        public void HandleCreateResponse(bool success)
        {

        }

        public void HandleLoginResponse(bool success, int sessionId)
        {
            _connection.SessionId = sessionId;


            if (success)
            {
                _connection.LoggedIn = LoginStatus.LoggedIn;
            }
            else
                _connection.LoggedIn = LoginStatus.LogginFailed;
        }
    }
}
