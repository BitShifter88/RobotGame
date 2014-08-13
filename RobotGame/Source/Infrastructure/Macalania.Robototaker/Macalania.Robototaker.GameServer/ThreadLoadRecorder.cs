using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Robototaker.GameServer
{
    class ThreadLoadRecorder
    {
        List<double> _load = new List<double>();

        public void RegisterLoad(double load)
        {
            _load.Add(load);

            if (_load.Count == 61)
                _load.RemoveAt(0);
        }

        public double GetSecondPeek()
        {
            double best = -1;

            foreach (double d in _load)
            {
                if (d > best)
                    best = d;
            }

            return best;
        }
    }
}
