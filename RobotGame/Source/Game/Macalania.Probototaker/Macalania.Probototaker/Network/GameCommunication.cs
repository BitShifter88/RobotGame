using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Macalania.Probototaker.Tanks;
using System.Threading;
using Frame.Network.Common;
using Frame.Network.Server;
using Macalania.Robototaker.Protocol;

namespace Macalania.Probototaker.Network
{
    public class PlayerMovement
    {
        public DrivingDirection DrivingDir;
        public RotationDirection RotationDir;

        public override bool Equals(object obj)
        {
            PlayerMovement pm = (PlayerMovement)obj;
            if (pm.DrivingDir == DrivingDir && pm.RotationDir == RotationDir)
                return true;
            return false;
        }
    }

    public class GameCommunication
    {
        GameNetwork _gn;
        PlayerMovement _lastPlayerMovement;
        Thread _sendThread;
        bool _stopThread;
        int _broadcastInterval = 50;
        int _broadcastIntervalCounter = 0;
        int _maxCountBroadcast = 3;
        byte _broadcastAttempt = 0;
        Mutex _broadcastMutex = new Mutex();
        int _countBroadcast = 0;
        int _commandCounter = 0;

        public GameCommunication(GameNetwork gn)
        {
            _gn = gn;
            
        }

        public void ReadyGameCommunication()
        {
            while (_gn.Authenticated == false)
            {
                Thread.Sleep(1);
            }

            _lastPlayerMovement = new PlayerMovement() { DrivingDir = DrivingDirection.Still, RotationDir = RotationDirection.Still };
            _commandCounter = 0;
            _countBroadcast = 0;
            if (_sendThread != null)
            {
                _stopThread = true;
                while(_sendThread.IsAlive)
                {
                    Thread.Sleep(1);
                }
            }

            _stopThread = false;
            _sendThread = new Thread(new ThreadStart(SendThread));
            _sendThread.Start();
        }

        private void SendPlayerMovement(PlayerMovement pm)
        {
            Message message = new Message();
            message.Write(_gn.GetClientUdp().Connection.Id);
            message.Write((byte)RobotProt.PlayerMovement);
            message.Write(_commandCounter);
            message.Write((byte)_countBroadcast);
            message.Write((byte)pm.DrivingDir);
            message.Write((byte)pm.RotationDir);

            _gn.GetClientUdp().Connection.SendMessage(message, AirUdpProt.Unsafe);
            Console.WriteLine("Broadcast. broadcastCount: " + _countBroadcast + "    Command counter: " + _commandCounter);
            _countBroadcast++;
            _broadcastIntervalCounter = _broadcastInterval;
        }

        private void SetLastPlayerMovement(PlayerMovement pm)
        {
            Console.WriteLine("New player movement");
            _lastPlayerMovement = pm;
            _countBroadcast = 0;
            _commandCounter++;
        }

        private void SendThread()
        {
            while (_stopThread)
            {
                while (_broadcastIntervalCounter > 0)
                {
                    Thread.Sleep(1);
                    _broadcastMutex.WaitOne();
                    _broadcastIntervalCounter--;
                    _broadcastMutex.ReleaseMutex();
                }

                _broadcastMutex.WaitOne();
                if (_broadcastIntervalCounter == 0)
                    Broadcast();
                _broadcastMutex.ReleaseMutex();
            }
        }

        private void Broadcast()
        {
            if (_countBroadcast == _maxCountBroadcast)
                return;

            SendPlayerMovement(_lastPlayerMovement);
        }

        public void Stop()
        {
            _stopThread = true;
        }

        public void PlayerMovement(PlayerMovement pm)
        {
            _broadcastMutex.WaitOne();
            if (pm.Equals(_lastPlayerMovement) == false)
            {
                SetLastPlayerMovement(pm);
                SendPlayerMovement(pm);
            }
            _broadcastMutex.ReleaseMutex();
        }
    }
}
