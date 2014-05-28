using System;
using System.Collections.Generic;

using System.Text;

namespace Frame.Network.SmartConnect
{
    class ResolvedSmartIP
    {
        private List<string> m_IPs;

        public List<string> IPs
        {
            get { return m_IPs; }
        }

        public ResolvedSmartIP(List<string> ips)
        {
            m_IPs = ips;
        }
    }
}
