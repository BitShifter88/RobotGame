using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Macalania.Robototaker.Log
{
    public enum LogType : byte
    {
        None = 0,
        CriticalError = 1,
        ConnectionStatus = 2,
        PossibleBug = 3,
        Information = 4,
        Security = 5,
        ServerOverload = 6,
        GameActivity = 7,
        Debug = 8,
        Lidgren = 9,
    }

    public static class ServerLog
    {
        private static bool _disabled = false;
        static Mutex _logMutex = new Mutex();

        public static void DisableConsole()
        {
            _disabled = true;
        }

        public static void EnableConsole()
        {
            _disabled = false;
        }





        public static void E(string message, LogType type)
        {
            if (_disabled)
                return;
            _logMutex.WaitOne();
            //Console.BackgroundColor = ConsoleColor.DarkGray;
            if (type == LogType.None)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (type == LogType.CriticalError)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (type == LogType.PossibleBug)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            else if (type == LogType.ConnectionStatus)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else if (type == LogType.Information)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (type == LogType.Security)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
            }
            else if (type == LogType.ServerOverload)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (type == LogType.GameActivity)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (type == LogType.Debug)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
            else if (type == LogType.Lidgren)
                Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine(DateTime.Now.ToString() + " ::\t" + message);
            Console.ForegroundColor = ConsoleColor.Gray;
            _logMutex.ReleaseMutex();
        }
    }
}
