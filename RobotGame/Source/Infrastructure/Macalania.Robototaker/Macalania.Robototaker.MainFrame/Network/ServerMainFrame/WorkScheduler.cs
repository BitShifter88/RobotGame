using Macalania.Robototaker.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Robototaker.MainFrame.Network.ServerMainFrame
{
    enum ServerRequestStatus
    {
        FindingServer,
        ServerHosted,
        Aborted,
    }
    class ServerRequest
    {
        public ServerRequestStatus Status;
        public string ServerIp;
    }
    class WorkScheduler
    {
        public static WorkScheduler Instance;

        ServerManager _serverManager;
        Dictionary<short, ServerRequest> _requests = new Dictionary<short, ServerRequest>();

        public WorkScheduler(ServerManager serverManager)
        {
            Instance = this;
            _serverManager = serverManager;
        }

        public void RequestServer(short gameId)
        {
            _requests.Add(gameId, new ServerRequest() { ServerIp = "", Status = ServerRequestStatus.FindingServer });

            _serverManager.GetServers()[0].RequestGameHosting(gameId);
        }

        public ServerRequest GetServerRequest(short gameId)
        {
            ServerRequest sr = _requests[gameId];

            if (sr.Status == ServerRequestStatus.ServerHosted)
                _requests.Remove(gameId);

            return sr;
        }

        public void ServerHosted(short gameId, string ip)
        {
            ServerLog.E("Game server hosted on ip " + ip, LogType.Information);
            _requests[gameId].ServerIp = ip;
            _requests[gameId].Status = ServerRequestStatus.ServerHosted;
        }
    }
}
