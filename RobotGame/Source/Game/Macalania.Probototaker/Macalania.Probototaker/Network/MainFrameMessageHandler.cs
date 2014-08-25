using Macalania.Robototaker.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Network
{
    class MainFrameMessageHandler : MainFrameMessageHandlerBase
    {
        public MainFrameMessageHandler(MainFrameConnection connection) : base (connection)
        {

        }

        public void HandleCreateResponse(bool success)
        {
            base.HandleCreateResponse(success);
        }

        public void HandleLoginResponse(bool success, int sessionId)
        {
            base.HandleLoginResponse(success, sessionId);
        }
    }
}
