using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Robototaker.Protocol
{
    public interface IMainFrameMessageHandler
    {
        public void HandleCreateResponse(bool success);
        public void HandleLoginResponse(bool success, int sessionId);
    }
}
