using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Robototaker.MainFrame
{
    static class SessionIdGenerator
    {
        static Random _ran = new Random();
        static Dictionary<int, DateTime> _usedSessionIds = new Dictionary<int, DateTime>();

        public static int GetNextId()
        {
            int ranId = _ran.Next(int.MinValue, int.MaxValue);

            while (true)
            {
                if (_usedSessionIds.Keys.Contains(ranId))
                {
                    ranId = _ran.Next(int.MinValue, int.MaxValue);
                }
                else
                    break;
            }

            _usedSessionIds.Add(ranId, DateTime.Now);

            return ranId;
        }

        public static void ReleaseId(int id)
        {
            _usedSessionIds.Remove(id);
        }
    }
}
