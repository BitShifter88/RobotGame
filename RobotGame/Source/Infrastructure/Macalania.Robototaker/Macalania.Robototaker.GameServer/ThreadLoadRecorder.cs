using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Robototaker.GameServer
{
    class ThreadLoadRecorder
    {
        List<double> _loadSec = new List<double>();
        List<double> _loadMin = new List<double>();

        public void RegisterLoad(double load)
        {
            _loadSec.Add(load);

            if (_loadSec.Count == 61)
                _loadSec.RemoveAt(0);

            _loadMin.Add(load);

            if (_loadMin.Count == 60 * 60 + 1)
                _loadMin.RemoveAt(0);
        }

        public double GetAvgMin()
        {
            double avg = 0;

            for (int i = 0; i < _loadMin.Count; i++ )
            {
                avg += _loadMin[i];
            }

            return avg/_loadMin.Count;
        }

        public double GetAvgSec()
        {
            double avg = 0;

            for (int i = 0; i < _loadSec.Count; i++ )
            {
                avg += _loadSec[i];
            }

            return avg/_loadSec.Count;
        }


        public double GetMinutPeek()
        {
            double best = -1;

            foreach (double d in _loadMin)
            {
                if (d > best)
                    best = d;
            }

            return best;
        }

        public double GetSecondPeek()
        {
            double best = -1;

            foreach (double d in _loadSec)
            {
                if (d > best)
                    best = d;
            }

            return best;
        }
    }
}
