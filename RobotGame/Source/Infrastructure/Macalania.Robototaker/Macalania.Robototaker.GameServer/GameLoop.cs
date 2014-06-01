using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Macalania.Robototaker.GameServer
{
    class GameLoop
    {
        Thread _loopThread;
        bool _stop = false;
        int _desiredFps = 60;
        public void StartGameLoop()
        {
            _loopThread = new Thread(new ThreadStart(LoopThread));
            _loopThread.Start();
        }

        protected virtual void Update(double dt)
        {
        }

        public void StopGameLoop()
        {
            _stop = true;
        }

        private void LoopThread()
        {
            Stopwatch elapsedTime = new Stopwatch();

            elapsedTime.Start();
            while (_stop == false)
            {
                
            }
        }
    }
}
