using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Network
{
    class GameRandom
    {
        public static float GetRandomFloat(float scale, Random random)
        {
            return (float)random.NextDouble() * scale;
        }

        public static int GetRandomInt(int min, int max, Random random)
        {
            return random.Next(min, max+1);
        }

        public static bool GetRandoBool(Random random)
        {
            int rnd = GetRandomInt(0, 1, random);
            if (rnd == 0)
                return false;
            else return true;
        }
    }
}
