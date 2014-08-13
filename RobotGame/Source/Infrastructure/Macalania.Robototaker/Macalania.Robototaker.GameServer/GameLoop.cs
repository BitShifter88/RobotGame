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
        ThreadLoadRecorder _load = new ThreadLoadRecorder();
       
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

                SecondUpdate();
            }
            
            //Thread.Sleep(10);
        }

        protected virtual void SecondUpdate()
        {
            ServerLog.E("Thread load: " + string.Format("{0:N2}%", _load.GetSecondPeek()), LogType.Information);
        }

        public void StopGameLoop()
        {
            _stop = true;
        }

        private void LoopThread()
        {
            Stopwatch elapsedTime = new Stopwatch();
            Stopwatch extraTimeWatch = new Stopwatch();
            Stopwatch load = new Stopwatch();
            double timeAtLastUpdate = 0;
            double frameTime = 1000d / 60d;
            elapsedTime.Start();

            while (_stop == false)
            {
                double dt = elapsedTime.Elapsed.TotalMilliseconds - timeAtLastUpdate;
                timeAtLastUpdate = elapsedTime.Elapsed.TotalMilliseconds;

                if (dt > frameTime + 0.01f || dt < frameTime - 0.01f)
                {
                    ServerLog.E("dt unstable", LogType.ServerOverload);
                }

                Update(dt);

                double timeToWait = _desiredUpdateTime - (elapsedTime.Elapsed.TotalMilliseconds - timeAtLastUpdate);
                load.Stop();
                
                double percentUsage = 1 - (frameTime - load.Elapsed.TotalMilliseconds) / frameTime;
                percentUsage *= 100;
                _load.RegisterLoad(percentUsage);
                load.Reset();

                if (timeToWait > 0)
                {
                    double extraSleepingTime = timeToWait - (double)((int)timeToWait);
              
                    //Thread.Sleep((int)timeToWait);

                    extraTimeWatch.Start();
                    
                    // PERFORMANCE: Det er lidt skidt at vil efterspørger TotalMiliseconds. Det bruger meget CPU. Overvej at lav et system med Sleep(1)
                    while (extraTimeWatch.Elapsed.TotalMilliseconds < timeToWait)
                        Thread.Sleep(0);
                    load.Start();
                    extraTimeWatch.Stop();
                    extraTimeWatch.Reset();
                }
                else
                {
                    load.Start();
                    ServerLog.E("Lag on the game loop!", LogType.ServerOverload);
                }
                //Console.WriteLine(timeToWait);
            }
        }
    }
}
