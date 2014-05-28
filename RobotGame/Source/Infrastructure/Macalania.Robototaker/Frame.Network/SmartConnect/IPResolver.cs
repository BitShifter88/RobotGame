using System;
using System.Collections.Generic;

using System.Text;
using System.Net;


namespace Frame.Network.SmartConnect
{
    static class IPResolver
    {
        public static ResolvedSmartIP Resolve(string smartIP)
        {
            ResolvedSmartIP resolved;
            List<string> ips = new List<string>();
            string[] elements = smartIP.Split(".".ToCharArray());

            for (int i = 0; i < elements.Length; i+=4)
            {
                string buffer = elements[i] + elements[i + 1] + elements[i + 2] + elements[i + 3];
                ips.Add(buffer);
            }
            resolved = new ResolvedSmartIP(ips);

            return resolved;
        }

        public static string GetSmartIP()
        {
            string smartIP = "";
            IPHostEntry IPHost = Dns.GetHostByName(Dns.GetHostName());
            IPHost.AddressList[0].ToString();
            foreach (IPAddress adress in IPHost.AddressList)
            {
                smartIP += adress.ToString() +".";
            }

            try
            {
                string WanIP = new System.Net.WebClient().DownloadString(("http://www.whatismyip.com/automation/n09230945.asp"));
                smartIP += WanIP;
            }
            catch (Exception e)
            {
                smartIP.Remove(smartIP.Length - 1);
            }
            return smartIP;
        }
    }
}
