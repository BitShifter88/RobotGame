using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Robototaker.Protocol
{
    public class MainFrameMessageParser
    {
        MainFrameMessageHandlerBase _messageHandler;

        public MainFrameMessageParser(MainFrameMessageHandlerBase handler)
        {
            _messageHandler = handler;
        }

        public void OnCreatePlayerResponse(NetIncomingMessage mr)
        {
            bool result = mr.ReadBoolean();
            _messageHandler.HandleCreateResponse(result);
        }

        public void OnAskIfReady(NetIncomingMessage mr)
        {
            short gameId = mr.ReadInt16();
            _messageHandler.AskIfReady(gameId);
        }

        public void OnGameHostSuccess(NetIncomingMessage mr)
        {
            short gameId = mr.ReadInt16();
            string serverIp = mr.ReadString();
            _messageHandler.GameHostSuccess(gameId, serverIp);
        }

        public void OnLoginResponse(NetIncomingMessage mr)
        {
            bool success = mr.ReadBoolean();
            int sessionId = 0;

            if (success)
            {
                sessionId = mr.ReadInt32();
            }

            _messageHandler.HandleLoginResponse(success, sessionId);
        }
    }
}
