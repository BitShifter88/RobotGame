using System;
using System.Collections.Generic;

using System.Text;
using System.IO;

namespace Frame.Network.Common
{
    public class MessageReader
    {
        private MemoryStream m_MemStream;
        private BinaryReader m_BinReader;
        private long m_SenderID;
        private bool m_HasRead;
        Message m_Message;

        public BinaryReader BinReader
        {
            get { m_HasRead = true; return m_BinReader; }
            set { m_BinReader = value; }
        }

        public long SenderID
        {
            get { return m_SenderID; }
            set { m_SenderID = value; }
        }

        public void SetNewMessage(Message message, long senderID)
        {
            m_Message = message;
            if (m_MemStream != null)
                m_MemStream.Dispose();
            PrepareMessage();
            m_SenderID = senderID;
        }

        private void PrepareMessage()
        {
            m_MemStream = new MemoryStream(m_Message.Data);
            m_BinReader = new BinaryReader(m_MemStream);
        }

        public void ResetMessageReadPositionIfHasRead(int byteOffset)
        {
            if (m_HasRead)
            {
                Dispose();
                PrepareMessage();
                m_BinReader.ReadBytes(byteOffset);
            }
        }

        public int ReadInt()
        {
            m_HasRead = true;
            return m_BinReader.ReadInt32();
        }

        public ushort ReadUshort()
        {
            m_HasRead = true;
            return m_BinReader.ReadUInt16();
        }

        public long ReadLong()
        {
            m_HasRead = true;
            return m_BinReader.ReadInt64();
        }

        public short ReadIntShort()
        {
            m_HasRead = true;
            return m_BinReader.ReadInt16();
        }

        public byte ReadByte()
        {
            m_HasRead = true;
            return m_BinReader.ReadByte();
        }

        public byte[] ReadBytes(int count)
        {
            m_HasRead = true;
            byte[] bytes = m_BinReader.ReadBytes(count);
            return bytes;
        }

        public float ReadFloat()
        {
            m_HasRead = true;
            return m_BinReader.ReadSingle();
        }

        public string ReadString()
        {
            m_HasRead = true;
            string s = BinaryHelper.ReadString(m_BinReader);
            return s;
        }

        public char ReadChar()
        {
            m_HasRead = true;
            char c = m_BinReader.ReadChar();
            return c;
        }

        public bool ReadBool()
        {
            m_HasRead = true;
            return m_BinReader.ReadBoolean();
        }

        public void Dispose()
        {
            m_MemStream.Dispose();
        }
    }
}
