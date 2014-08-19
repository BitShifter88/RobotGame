using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Robototaker.MainFrame.Network.GameMainFrame
{
    class GServer
    {
        public NetPeerConfiguration Config { get; set; }
        NetServer _netServer;

        public void Start(int port)
        {
            Config = new NetPeerConfiguration("game");
#if DEBUG
            Config.SimulatedMinimumLatency = 0.015f;
#endif

            Config.Port = port;
            Config.MaximumConnections = 2000;
            Config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            _netServer = new NetServer(Config);
            _netServer.Start();
        }
    }
}
