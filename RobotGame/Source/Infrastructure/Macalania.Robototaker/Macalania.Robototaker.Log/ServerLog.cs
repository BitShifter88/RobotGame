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
        ServerOverload = 6,
        GameActivity = 7,
        Debug = 8,
    }

    public static class ServerLog
    {
        private static int _consoleWindowHeight = 0;
        private static int _consoleWindowStart = 0;
        private static bool _disabled = false;

        public static void DisableConsole()
        {
            _disabled = true;
        }

        public static void EnableConsole()
        {
            _disabled = false;
        }

        public static void CreateConsoleWindow(int height)
        {
            SetConsoleWindow(height);
        }

        private static void SetConsoleWindow(int height)
        {
#if !RASPBERRY
            _consoleWindowHeight = height;
            _consoleWindowStart = Console.CursorTop;
            Console.SetCursorPosition(0, _consoleWindowStart + height);
#endif
        }

        public static void ClearConsoleWindow()
        {
#if !RASPBERRY
            if (_disabled)
                return;
            int top = Console.CursorTop;
            int left = Console.CursorLeft;
            for (int i = _consoleWindowStart; i < _consoleWindowStart + _consoleWindowHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("                                             ");
            }
            Console.SetCursorPosition(left, top);
#endif
        }

        public static void WriteToConsoleWindow(string text, int row)
        {
            if (_disabled)
                return;
#if !RASPBERRY
            int top = Console.CursorTop;
            int left = Console.CursorLeft;

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, row + _consoleWindowStart);
#endif
            Console.WriteLine(text);
#if !RASPBERRY
            Console.SetCursorPosition(left, top);
#endif
        }

        public static void E(string message, LogType type)
        {
            if (_disabled)
                return;
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

            Console.WriteLine(DateTime.Now.ToString() + " ::\t" + message);

#if !RASPBERRY
            _consoleWindowStart = Console.CursorTop;
            Console.SetCursorPosition(0, _consoleWindowStart + _consoleWindowHeight);
#endif
        }
    }
}
