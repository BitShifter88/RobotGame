using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Macalania.Probototaker.Tanks;
using System.Threading;
using Macalania.Robototaker.Protocol;
using Lidgren.Network;
using Frame.Network.Common;
using Macalania.Probototaker.Tanks.Plugins;
using Microsoft.Xna.Framework;

namespace Macalania.Probototaker.Network
{
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
            _lastPlayerMovement = new PlayerMovement() { DrivingDir = DrivingDirection.Still, BodyDir = RotationDirection.Still };
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
            NetOutgoingMessage message = _gn.GetClientUdp().CreateMessage();

            message.Write((byte)RobotProt.PlayerMovement);

            byte firering = 0;

            if (pm.MainGunFirering == true)
                firering = 1;

            byte packed = BytePacker.Pack((byte)pm.DrivingDir, (byte)pm.BodyDir, (byte)pm.TurretDir, firering);
            message.Write(packed);

            message.Write(pm.TurretRotation);

            message.Write(_commandCounter);
            message.Write((byte)_countBroadcast);

            _gn.GetClientUdp().SendMessage(message, NetDeliveryMethod.Unreliable, 0);
            Console.WriteLine("Broadcast. broadcastCount: " + _countBroadcast + "    Command counter: " + _commandCounter);
            _countBroadcast++;
            _broadcastIntervalCounter = _broadcastInterval;
        }

        public void SendAbilityActivation(PluginType type, Tank targetTank, Vector2 targetPosition)
        {
            NetOutgoingMessage message = _gn.GetClientUdp().CreateMessage();

            message.Write((byte)RobotProt.AbilityActivation);
            message.Write((byte)type);

            if (type == PluginType.ArtileryStart)
            {
                message.Write(targetPosition.X);
                message.Write(targetPosition.Y);
            }
            

            _gn.GetClientUdp().SendMessage(message, NetDeliveryMethod.ReliableUnordered);
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
