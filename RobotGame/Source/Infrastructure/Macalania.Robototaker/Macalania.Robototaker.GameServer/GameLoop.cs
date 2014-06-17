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
        double _desiredUpdateTime = 1000d / 60d;
       
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
            
            //Thread.Sleep(10);
        }

        public void StopGameLoop()
        {
            _stop = true;
        }

        private void LoopThread()
        {
            Stopwatch elapsedTime = new Stopwatch();
            Stopwatch extraTimeWatch = new Stopwatch();
            Stopwatch performance = new Stopwatch();
            double timeAtLastUpdate = 0;
            elapsedTime.Start();
            while (_stop == false)
            {
                double dt = elapsedTime.Elapsed.TotalMilliseconds - timeAtLastUpdate;
                timeAtLastUpdate = elapsedTime.Elapsed.TotalMilliseconds;
                performance.Start();
                Update(dt);
                performance.Stop();
                //ServerLog.E((performance.Elapsed.TotalMilliseconds * 1000).ToString(), LogType.Debug);
                performance.Reset();

                double timeToWait = _desiredUpdateTime - (elapsedTime.Elapsed.TotalMilliseconds - timeAtLastUpdate);

                if (timeToWait > 0)
                {
                    double extraSleepingTime = timeToWait - (double)((int)timeToWait);
              
                    //Thread.Sleep((int)timeToWait);

                    extraTimeWatch.Start();
                    while (extraTimeWatch.Elapsed.TotalMilliseconds < timeToWait)
                        Thread.Sleep(0);
                    extraTimeWatch.Stop();
                    extraTimeWatch.Reset();
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
