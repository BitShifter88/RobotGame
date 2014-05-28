using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }

    public static class ServerLog
    {
        public static void E(string message, LogType type)
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;
            if (type == LogType.None)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (type == LogType.CriticalError)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
            }
            else if (type == LogType.PossibleBug)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            }
            else if (type == LogType.ConnectionStatus)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
            }
            else if (type == LogType.Information)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            }
            else if (type == LogType.Security)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
            }

            Console.WriteLine(DateTime.Now.ToString() + " ::\t" + message);
        }
    }
}
