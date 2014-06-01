using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Network
{
    class GameRandom
    {
        static Random _random = new Random();

        public static float GetRandomFloat(float scale)
        {
            return (float)_random.NextDouble() * scale;
        }
    }
}
