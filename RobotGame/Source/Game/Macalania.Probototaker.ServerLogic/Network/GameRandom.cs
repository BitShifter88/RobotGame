using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Network
{
    public class GameRandom
    {
        static Random _random = new Random();

        public static float GetRandomFloat(float scale)
        {
            return (float)_random.NextDouble() * scale;
        }

        public static int GetRandomInt(int min, int max)
        {
            return _random.Next(min, max+1);
        }

        public static bool GetRandoBool()
        {
            int rnd = GetRandomInt(0, 1);
            if (rnd == 0)
                return false;
            else return true;
        }
    }
}
