using System;
using System.Collections.Generic;

using System.Text;
using System.IO;

namespace Frame.Network.Common
{
    public class Message
    {
        MemoryStream memStream;
        BinaryWriter binWriter;

        public byte[] Data
        {
            get { return memStream.ToArray(); }
        }

        public Message()
        {
            memStream = new MemoryStream();
            binWriter = new BinaryWriter(memStream);
        }

        public Message(byte[] data)
        {
            memStream = new MemoryStream(data);
            binWriter = new BinaryWriter(memStream);
        }

        public void Write(int value)
        {
            binWriter.Write(BitConverter.GetBytes(value));
        }

        public void Write(long value)
        {
            binWriter.Write(BitConverter.GetBytes(value));
        }

        public void Write(short value)
        {
            binWriter.Write(BitConverter.GetBytes(value));
        }

        public void Write(ushort value)
        {
            binWriter.Write(BitConverter.GetBytes(value));
        }

        public void Write(byte value)
        {
            binWriter.Write(value);
        }

        public void Write(byte[] value)
        {
            binWriter.Write(value);
        }

        public void Write(float value)
        {
            binWriter.Write(BitConverter.GetBytes(value));
        }

        public void Write(bool value)
        {
            binWriter.Write(BitConverter.GetBytes(value));
        }

        public void Write(string value)
        {
            BinaryHelper.WriteString(value, binWriter);
        }

        public void Dispose()
        {
            memStream.Dispose();
        }
    }
}
