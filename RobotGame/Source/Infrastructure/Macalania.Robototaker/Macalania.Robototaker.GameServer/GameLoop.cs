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
                ServerLog.CreateConsoleWindow(5);
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
                ServerLog.ClearConsoleWindow();
                ServerLog.WriteToConsoleWindow("FPS: " + frames, 0);
                ServerLog.WriteToConsoleWindow("Thread load peek sec: " + string.Format("{0:N2}%", _load.GetSecondPeek()), 1);
                ServerLog.WriteToConsoleWindow("Thread load peek min: " + string.Format("{0:N2}%", _load.GetMinutPeek()), 2);
                ServerLog.WriteToConsoleWindow("Thread load avg sec: " + string.Format("{0:N2}%", _load.GetAvgSec()), 3);
                ServerLog.WriteToConsoleWindow("Thread load avg min: " + string.Format("{0:N2}%", _load.GetAvgMin()), 4);
            }
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

            double dt ;
            double timeToWait;
            double percentUsage;
            double extraSleepingTime;

            while (_stop == false)
            {
                dt = elapsedTime.Elapsed.TotalMilliseconds - timeAtLastUpdate;
                timeAtLastUpdate = elapsedTime.Elapsed.TotalMilliseconds;

                //if (dt > frameTime + 0.01f || dt < frameTime - 0.01f)
                //{
                //    ServerLog.E("dt unstable", LogType.ServerOverload);
                //}

                Update(dt);

                timeToWait = _desiredUpdateTime - (elapsedTime.Elapsed.TotalMilliseconds - timeAtLastUpdate);
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
