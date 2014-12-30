using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Robototaker.MainFrame
{
    class IdGenerator
    {
        static Random _ran = new Random();
        static Dictionary<short, DateTime> _usedIds = new Dictionary<short, DateTime>();

        public static short GetNextId()
        {
            short ranId = (short)_ran.Next(short.MinValue, short.MaxValue);

            while (true)
            {
                if (_usedIds.Keys.Contains(ranId))
                {
                    ranId = (short)_ran.Next(short.MinValue, short.MaxValue);
                }
                else
                    break;
            }

            _usedIds.Add(ranId, DateTime.Now);

            return ranId;
        }

        public static void ReleaseId(short id)
        {
            _usedIds.Remove(id);
        }
    }
}
