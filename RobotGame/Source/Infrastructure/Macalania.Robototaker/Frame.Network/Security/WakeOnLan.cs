using System;
using System.Collections.Generic;

using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Globalization;

namespace Frame.Network.Security
{
    public static class WakeOnLan
    {
        public static void WOL(byte[] targetMAC, byte[] password)
        {
            // target mac must be 6-bytes!
            if (targetMAC.Length != 6)
            {
                throw new ArgumentException();
            }

            // check password
            if ((password != null) &&
                (password.Length != 4) &&
                (password.Length != 6))
            {
                throw new ArgumentException();
            }

            int packetLength = 6 + (20 * 6);
            if (password != null)
            {
                packetLength += password.Length;
            }

            byte[] magicPacket = new byte[packetLength];

            // has a 6-byte header of 0xFF
            byte[] header = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            Buffer.BlockCopy(header, 0, magicPacket, 0, header.Length);

            // repeat the destination MAC 16 times
            // your MAC *is* in network (reverse) order, right??
            int offset = 6;
            for (int i = 0; i < 16; i++)
            {
                Buffer.BlockCopy(targetMAC, 0, magicPacket, offset, targetMAC.Length);
                offset += 6;
            }

            if (password != null)
            {
                Buffer.BlockCopy(password, 0, magicPacket, offset, password.Length);
            }

            IPEndPoint ep = new IPEndPoint(IPAddress.Broadcast, 7);
            UdpClient c = new UdpClient();
            c.Send(magicPacket, magicPacket.Length, ep);
        }
    }
}
