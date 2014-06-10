using Macalania.Robototaker.Log;
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
        double _desiredUpdateTime = 1000 / 60;
       
        public void StartGameLoop()
        {
            ServerLog.E("Supports High Resolution Clock: " + Stopwatch.IsHighResolution, LogType.Information);

            _loopThread = new Thread(new ThreadStart(LoopThread));
            _loopThread.Start();
        }

        int frames = 0;
        Stopwatch frameCounter;

        protected virtual void Update(double dt)
        {
            frames++;

            if (frameCounter == null)
            {
                frameCounter = new Stopwatch();
                frameCounter.Start();
            }

            if (frameCounter.Elapsed.TotalMilliseconds >= 1000)
            {
                frameCounter.Reset();
                frameCounter.Start();
                ServerLog.E("FPS: " + frames, LogType.Debug);
                frames = 0;
            }
            
            Thread.Sleep(2);
        }

        public void StopGameLoop()
        {
            _stop = true;
        }

        private void LoopThread()
        {
            Stopwatch elapsedTime = new Stopwatch();
            double timeAtLastUpdate = 0;
            double timeMissedSleeping = 0;
            elapsedTime.Start();
            while (_stop == false)
            {
                double dt = elapsedTime.Elapsed.TotalMilliseconds - timeAtLastUpdate;
                timeAtLastUpdate = elapsedTime.Elapsed.TotalMilliseconds;
                Update(dt);

                double timeToWait = _desiredUpdateTime - (elapsedTime.Elapsed.TotalMilliseconds - timeAtLastUpdate);

                if (timeToWait > 0)
                {
                    timeMissedSleeping += timeToWait - (double)((int)timeToWait);

                    int extraTime = 0;

                    if (timeMissedSleeping >= 1)
                    {
                        extraTime = 1;
                        timeMissedSleeping -= 1;
                    }
                    
                    Thread.Sleep((int)timeToWait + extraTime);
                }
                else
                {
                    ServerLog.E("Lag on the game loop!", LogType.ServerOverload);
                }
                //Console.WriteLine(timeToWait);
            }
        }
    }
}
