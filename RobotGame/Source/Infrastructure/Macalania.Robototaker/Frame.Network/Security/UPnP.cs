using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Xml;
using System.IO;

namespace Frame.Network.Security
{
    public delegate void Method();
    public class UPnP
    {
        public static string Discover()
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            string req = "M-SEARCH * HTTP/1.1\r\n" +
            "HOST: 239.255.255.250:1900\r\n" +
            "ST:upnp:rootdevice\r\n" +
            "MAN:\"ssdp:discover\"\r\n" +
            "MX:3\r\n\r\n";
            byte[] data = Encoding.ASCII.GetBytes(req);

            byte[] buffer = new byte[0x1000];

            string resp;

            do
            {
                s.SendTo(data, new IPEndPoint(IPAddress.Broadcast, 1900));
                s.SendTo(data, new IPEndPoint(IPAddress.Broadcast, 1900));
                s.SendTo(data, new IPEndPoint(IPAddress.Broadcast, 1900));

                int l = s.Receive(buffer);

                resp = Encoding.ASCII.GetString(buffer, 0, l);

                if (resp.ToLower().Contains("upnp:rootdevice"))
                {
                    resp = resp.Substring(resp.ToLower().IndexOf("location:") + "Location:".Length);
                    resp = resp.Substring(0, resp.IndexOf("\r")).Trim();
                    if (resp.ToLower().Contains(".xml"))
                    {
                        descUrl = resp;
                        GetServiceUrl();
                        break;
                    }
                }
            } while (true);

            return resp;
        }


        static string descUrl;
        static string serviceUrl;

        static void GetServiceUrl()
        {
            XmlDocument desc = new XmlDocument();
            desc.Load(WebRequest.Create(descUrl).GetResponse().GetResponseStream());
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(desc.NameTable);
            nsMgr.AddNamespace("tns", "urn:schemas-upnp-org:device-1-0");
            XmlNode node = desc.SelectSingleNode("//tns:service[tns:serviceType=\"urn:schemas-upnp-org:service:WANIPConnection:1\"]/tns:controlURL/text()", nsMgr);
            if (node != null)
            {
                string relurl = node.Value;
                int n;
                if (relurl[0] == '/')
                {
                    n = descUrl.IndexOf("://");
                    if (n > -1)
                    {
                        n = descUrl.IndexOf('/', n + 3);
                        if (n > -1) serviceUrl = descUrl.Substring(0, n) + relurl;
                    }
                }
                else
                {
                    serviceUrl = descUrl.Substring(0, descUrl.LastIndexOf("/")) + relurl;
                }
                // serviceUrl = descUrl.Substring(0, descUrl.LastIndexOf("/")) + relurl;
            }
        }
        public static IPAddress GetExternalIP()
        {
            if (string.IsNullOrEmpty(serviceUrl))
                throw new Exception("No UPnP service available or Discover() has not been called");
            XmlDocument xdoc = SOAPRequest(serviceUrl, "<u:GetExternalIPAddress xmlns:u=\"urn:schemas-upnp-org:service:WANIPConnection:1\">\r\n" +
            "</u:GetExternalIPAddress>\r\n", "GetExternalIPAddress");
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(xdoc.NameTable);
            nsMgr.AddNamespace("tns", "urn:schemas-upnp-org:device-1-0");
            string IP = xdoc.SelectSingleNode("//NewExternalIPAddress/text()", nsMgr).Value;
            return IPAddress.Parse(IP);
        }
        public static void ForwardPort(int port, ProtocolType protocol, string description)
        {
            if (string.IsNullOrEmpty(serviceUrl))
                throw new Exception("No UPnP service available or Discover() has not been called");
            XmlDocument xdoc = SOAPRequest(serviceUrl, "<u:AddPortMapping xmlns:u=\"urn:schemas-upnp-org:service:WANIPConnection:1\">\r\n" +
                "<NewRemoteHost></NewRemoteHost><NewExternalPort>" + port.ToString() + "</NewExternalPort><NewProtocol>" + protocol.ToString().ToUpper() + "</NewProtocol>" +
                "<NewInternalPort>" + port.ToString() + "</NewInternalPort><NewInternalClient>" + Dns.GetHostAddresses(Dns.GetHostName())[0].ToString() +
                "</NewInternalClient><NewEnabled>1</NewEnabled><NewPortMappingDescription>" + description +
            "</NewPortMappingDescription><NewLeaseDuration>0</NewLeaseDuration></u:AddPortMapping>\r\n", "AddPortMapping");
        }
        //warning: experimental - does not work for my router
        public static void DeleteForwardingRule(int port, ProtocolType protocol)
        {
            if (string.IsNullOrEmpty(serviceUrl))
                throw new Exception("No UPnP service available or Discover() has not been called");
            XmlDocument xdoc = SOAPRequest(serviceUrl,
            "<u:DeletePortMapping xmlns:u=\"urn:schemas-upnp-org:service:WANIPConnection:1\">\r\n" +
            "<NewRemoteHost>\r\n" +
            "</NewRemoteHost>\r\n" +
            "<NewExternalPort>1234</NewExternalPort>\r\n" +
            "<NewProtocol>\r\n" + protocol.ToString().ToUpper() + "</NewProtocol>\r\n" +
            "</u:DeletePortMapping>\r\n", "DeletePortMapping");
        }
        public static XmlDocument GetPortMappingEntry(int index)
        {
            return SOAPRequest(serviceUrl,
            "<u:GetGenericPortMappingEntry xmlns:u=\"urn:schemas-upnp-org:service:WANIPConnection:1\">\r\n" +
            "<NewPortMappingIndex>" + index + "</NewPortMappingIndex>\r\n" +
            "</u:GetGenericPortMappingEntry>\r\n", "GetGenericPortMappingEntry");
        }
        static XmlDocument SOAPRequest(string url, string soap, string function)
        {
            string req = "<?xml version=\"1.0\"?>\r\n" +
            "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n" +
            "<s:Body>\r\n" +
            soap +
            "</s:Body>\r\n" +
            "</s:Envelope>";
            WebRequest r = HttpWebRequest.Create(url);
            r.Method = "POST";
            byte[] b = Encoding.UTF8.GetBytes(req);
            r.Headers.Add("SOAPACTION", "\"urn:schemas-upnp-org:service:WANIPConnection:1#" + function + "\"");
            r.ContentType = "text/xml; charset=\"utf-8\"";
            r.ContentLength = b.Length;
            r.GetRequestStream().Write(b, 0, b.Length);
            XmlDocument resp = new XmlDocument();
            WebResponse wres = r.GetResponse();
            Stream ress = wres.GetResponseStream();
            resp.Load(ress);
            return resp;
        }
    }
}
