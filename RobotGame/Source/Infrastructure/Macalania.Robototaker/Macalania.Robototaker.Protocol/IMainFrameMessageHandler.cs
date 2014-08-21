using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Robototaker.Protocol
{
    public interface IMainFrameMessageHandler
    {
        void HandleCreateResponse(bool success);
        void HandleLoginResponse(bool success, int sessionId);
    }
}
