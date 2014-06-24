using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Frame.Network.Common
{
    public class BytePacker
    {
        public static byte GetFirst(byte value)
        {
            value = (byte)((int)value << 6);
            value = (byte)((int)value >> 6);

            return value;
        }

        public static byte GetSecond(byte value)
        {
            value = (byte)((int)value << 4);
            value = (byte)((int)value >> 6);

            return value;
        }

        public static byte GetThird(byte value)
        {
            value = (byte)((int)value << 2);
            value = (byte)((int)value >> 6);

            return value;
        }

        public static byte GetFourth(byte value)
        {
            value = (byte)((int)value >> 6);

            return value;
        }

        public static byte Pack(byte b1, byte b2, byte b3, byte b4)
        {
            byte value = 0;

            value += b4;
            value = (byte)((int)value << 2);

            value += b3;
            value = (byte)((int)value << 2);

            value += b2;
            value = (byte)((int)value << 2);

            value += b1;

            return value;
        }
    }
}
