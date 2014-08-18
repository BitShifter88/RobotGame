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
        public bool ShowStats { get; set; }
       
        public void StartGameLoop()
        {
            ServerLog.E("Supports High Resolution Clock: " + Stopwatch.IsHighResolution, LogType.Information);
            ServerLog.E("GameLoop started!", LogType.Information);
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

            if (frameCounter.ElapsedMilliseconds >= 1000)
            {
                frameCounter.Reset();
                frameCounter.Start();

                SecondUpdate();

                frames = 0;
            }
            
            //Thread.Sleep(10);
        }

        bool displayUpdate = false;
        protected virtual void SecondUpdate()
        {
            if (ShowStats)
            {
                displayUpdate = true;
                ServerLog.E("FPS: " + fps, LogType.Information);
                ServerLog.E("Thread load peek sec: " + string.Format("{0:N2}%", _load.GetSecondPeek()), LogType.Information);
                ServerLog.E("Thread load peek min: " + string.Format("{0:N2}%", _load.GetMinutPeek()), LogType.Information);
                ServerLog.E("Thread load avg sec: " + string.Format("{0:N2}%", _load.GetAvgSec()), LogType.Information);
                ServerLog.E("Thread load avg min: " + string.Format("{0:N2}%", _load.GetAvgMin()), LogType.Information);
            }
        }

        public void StopGameLoop()
        {
            _stop = true;
        }

        double fps = 0;

        private void LoopThread()
        {
            Stopwatch elapsedTime = new Stopwatch();
            Stopwatch extraTimeWatch = new Stopwatch();
            Stopwatch load = new Stopwatch();
            double timeAtLastUpdate = 0;
            double frameTime = 1000d / 60d;
            elapsedTime.Start();

            double dt ;
            double timeToWait;
            double percentUsage;
            double extraSleepingTime;
            double correction = 0;
            double restCorrection = 0;

            bool first = true;
            int frames = 0;

            while (_stop == false)
            {
                dt = elapsedTime.Elapsed.TotalMilliseconds - timeAtLastUpdate + correction;
                timeAtLastUpdate = elapsedTime.Elapsed.TotalMilliseconds;

                correction = dt - frameTime;
                correction += restCorrection;
                restCorrection = 0;

                Update(frameTime);
                frames++;
                fps = 1000d / (elapsedTime.Elapsed.TotalMilliseconds / (double)frames);

                timeToWait = _desiredUpdateTime - (elapsedTime.Elapsed.TotalMilliseconds - timeAtLastUpdate);
                timeToWait -= correction;
                if (timeToWait < 0)
                {
                    restCorrection += Math.Abs(timeToWait);
                    correction -= Math.Abs(timeToWait);
                    timeToWait = 0;
                    //ServerLog.E("Doing correctionRest", LogType.Information);
                }

                load.Stop();
                
                percentUsage = 1 - (frameTime - load.Elapsed.TotalMilliseconds) / frameTime;
                percentUsage *= 100;
                if (displayUpdate == false)
                {
                    _load.RegisterLoad(percentUsage);
                }
                else
                    displayUpdate = false;
                load.Reset();

                if (timeToWait > 0)
                {
                    extraSleepingTime = timeToWait - (double)((int)timeToWait);
                    
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
                    //ServerLog.E("Lag on the game loop!", LogType.ServerOverload);
                }
                first = false;
                //Console.WriteLine(timeToWait);
            }
        }
    }
}
