using Lidgren.Network;
using Macalania.Robototaker.Log;
using Macalania.Robototaker.Protocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Macalania.Robototaker.GameServer
{
    public enum AuthorizedStatus
    {
        NotAuthorized,
        Authorized,
        AuthorizeFailed,
        AuthorizeTimedOut,
    }

    class MainFrameConnection
    {
        GameServerManager _gsm;
        NetClient _client;
        int _authenticationTimeout = 5000;
        bool _stop = false;
        Thread _messageThread;
        public AuthorizedStatus AuthorizeStatus { get; set; }

        public MainFrameConnection(GameServerManager gsm)
        {
            AuthorizeStatus = AuthorizedStatus.NotAuthorized;
            _gsm = gsm;
        }

        public bool ConnectToMainFrame(int port)
        {
            ServerLog.E("Connecting to Main Frame...", LogType.Information);

            NetPeerConfiguration Config = new NetPeerConfiguration("game");

            _client = new NetClient(Config);

            _client.Start();

            _client.Connect("127.0.0.1", port);

            if (WaitForAuthentication() == false)
            {
                ServerLog.E("Could not connect to Main Frame!", LogType.ConnectionStatus);

                return false;
            }
            else
            {
                ServerLog.E("Connected to Main Frame!", LogType.ConnectionStatus);
                _messageThread = new Thread(new ThreadStart(ReadMessages));
                _messageThread.Start();
                //_messageThread = new Thread(new ThreadStart(ReadMessages));
                //_messageThread.Start();
            }
            return true;
        }

        public void Authorize(string username, string password)
        {
            ServerLog.E("Trying to authorize server...", LogType.Security);
            NetOutgoingMessage message = _client.CreateMessage();
            message.Write((byte)InfrastructureProt.Authorize);
            message.Write(username);
            message.Write(password);
            _client.SendMessage(message, NetDeliveryMethod.ReliableUnordered);
        }

        public AuthorizedStatus WaitForAuthenticationResponse(int timeout)
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            while (s.ElapsedMilliseconds < timeout)
            {
                if (AuthorizeStatus == AuthorizedStatus.Authorized || AuthorizeStatus == AuthorizedStatus.AuthorizeFailed)
                    return AuthorizeStatus;
            }

            AuthorizeStatus = AuthorizedStatus.AuthorizeTimedOut;

            return AuthorizeStatus;
        }

        private void OnAuthorizeResponse(NetIncomingMessage mr)
        {
            bool success = mr.ReadBoolean();

            if (success)
                AuthorizeStatus = AuthorizedStatus.Authorized;
            else
                AuthorizeStatus = AuthorizedStatus.AuthorizeFailed;
        }

        private void ReadMessages()
        {
            while (_stop == false)
            {
                NetIncomingMessage mr;

                if ((mr = _client.ReadMessage()) != null)
                {
                    switch (mr.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            {
                                InfrastructureProt header = (InfrastructureProt)mr.ReadByte();

                                if (header == InfrastructureProt.Authorize)
                                    OnAuthorizeResponse(mr);

                                //if (header == MainFrameProt.CreatePlayer)
                                //{
                                //    _parser.OnCreatePlayerResponse(mr);
                                //}
                                //else if (header == MainFrameProt.Login)
                                //{
                                //    _parser.OnLoginResponse(mr);
                                //}
                            }
                            break;
                    }
                    _client.Recycle(mr);
                }
                else
                    Thread.Sleep(1);

            }
        }

        private bool WaitForAuthentication()
        {
            bool authenticated = false;
            NetIncomingMessage inc;
            Stopwatch s = new Stopwatch();
            s.Start();

            while (s.ElapsedMilliseconds <= _authenticationTimeout)
            {
                if (_client.ConnectionStatus == NetConnectionStatus.Connected)
                    break;
                if ((inc = _client.ReadMessage()) != null)
                {
                    switch (inc.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            {
                                authenticated = true;
                                ServerLog.E("Authenticated!", LogType.Security);
                            }
                            break;
                        case NetIncomingMessageType.WarningMessage:
                            {
                                ServerLog.E(inc.ReadString(), LogType.Lidgren);
                            }
                            break;
                        case NetIncomingMessageType.Error:
                            {
                                ServerLog.E(inc.ReadString(), LogType.Lidgren);
                            }
                            break;
                        case NetIncomingMessageType.VerboseDebugMessage:
                            {
                                ServerLog.E(inc.ReadString(), LogType.Lidgren);
                            }
                            break;
                        case NetIncomingMessageType.DebugMessage:
                            {
                                ServerLog.E(inc.ReadString(), LogType.Lidgren);
                            }
                            break;
                    }
                }
                else
                    Thread.Sleep(1);
            }

            if (s.Elapsed.TotalMilliseconds >= 5000)
                return false;
            return true;
        }
    }
}
