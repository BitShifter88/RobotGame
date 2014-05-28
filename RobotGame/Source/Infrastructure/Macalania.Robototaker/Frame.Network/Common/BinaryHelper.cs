using System;
using System.Collections.Generic;

using System.Text;
using System.IO;

namespace Frame.Network.Common
{
    class BinaryHelper
    {
        public static void WriteString(string str, BinaryWriter bw)
        {
            bw.Write(BitConverter.GetBytes(str.Length));
            for (int i = 0; i < str.Length; i++)
            {
                bw.Write(BitConverter.GetBytes(str[i]));
            }
        }

        public static string ReadString(BinaryReader br)
        {
            int lenght = br.ReadInt32();
            char[] buffer = new char[lenght];
            for (int i = 0; i < lenght; i++)
            {
                byte[] b = new byte[2];
                b[0] = br.ReadByte();
                b[1] = br.ReadByte();
                buffer[i] = BitConverter.ToChar(b, 0);
            }
            return new string(buffer);
        }

        public static string ReadString(byte[] bytes, int startIndex, out int byteSize)
        {
            int lenght = BitConverter.ToInt32(bytes, startIndex);
            byteSize = 4 + lenght * sizeof(char);
            char[] buffer = new char[lenght];
            for (int i = 0; i < lenght; i++)
            {
                buffer[i] = BitConverter.ToChar(bytes, startIndex + 4 + (i * 2));
            }
            return new string(buffer);
        }
    }
}
