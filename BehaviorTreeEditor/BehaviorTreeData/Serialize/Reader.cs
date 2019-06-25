using System;
using System.Collections.Generic;
using System.IO;

namespace BehaviorTreeData
{
    public class Reader
    {
        public Reader()
        {
            m_stream = new MemoryStream();
            m_binaryReader = new BinaryReader(m_stream);
        }

        BinaryReader m_binaryReader = null;
        MemoryStream m_stream = null;
        int m_index = 0;
        byte[] m_buffer;

        public void Load(byte[] data, int index, int size)
        {
            m_stream.Write(data, index, size);
            m_stream.Position = 0;
            m_index = 0;
            m_buffer = m_stream.GetBuffer();
        }

        public BinaryReader reader
        {
            get { return m_binaryReader; }
        }

        public MemoryStream stream
        {
            get { return m_stream; }
        }

        public int index
        {
            set { m_index = value; }
            get { return m_index; }
        }

        public BinaryReader binaryReader
        {
            get { return m_binaryReader; }
        }

        public byte ReadByte()
        {
            byte value = m_binaryReader.ReadByte();
            m_index++;
            return value;
        }

        public UInt16 ReadUInt16()
        {
            UInt16 value = m_binaryReader.ReadUInt16();
            m_index += 2;
            return value;
        }

        public Reader ReadBoolean(ref Boolean value)
        {
            if (m_index < m_buffer.Length)
            {
                UInt32 temp = ReadUInt32Variant();
                value = temp == 1;
            }

            return this;
        }

        public Reader ReadRepeatedBoolean(ref List<Boolean> value)
        {
            if (m_index < m_buffer.Length)
            {
                uint count = ReadUInt32Variant();

                if (count > 0)
                {
                    if (value == null)
                        value = new List<bool>((int)count);

                    for (int i = 0; i < count; i++)
                    {
                        UInt32 temp = ReadUInt32Variant();
                        value.Add(temp == 1);
                    }
                }
            }

            return this;
        }

        public Reader ReadEnum(ref int Value)
        {
            if (m_index < m_buffer.Length)
            {
                Value = ReadInt32Variant();
            }

            return this;
        }

        public Reader ReadInt32(ref Int32 value)
        {
            if (m_index < m_buffer.Length)
            {
                value = ReadInt32Variant();
            }

            return this;
        }

        public Reader ReadRepeatedInt32(uint key, ref List<Int32> value)
        {
            if (m_index < m_buffer.Length)
            {
                uint count = ReadUInt32Variant();
                if (count > 0)
                {
                    if (value == null)
                        value = new List<int>((int)count);

                    for (int i = 0; i < count; i++)
                    {
                        Int32 temp = ReadInt32Variant();
                        value.Add(temp);
                    }
                }
            }

            return this;
        }

        public Reader ReadUInt32(ref UInt32 value)
        {
            if (m_index < m_buffer.Length)
            {
                value = ReadUInt32Variant();
            }
            return this;
        }

        public Reader ReadRepeatedUInt32(ref List<UInt32> value)
        {
            if (m_index < m_buffer.Length)
            {
                uint count = ReadUInt32Variant();
                if (count > 0)
                {
                    if (value == null)
                        value = new List<uint>((int)count);

                    for (int i = 0; i < count; i++)
                    {
                        UInt32 temp = ReadUInt32Variant();
                        value.Add(temp);
                    }
                }
            }

            return this;
        }

        public Reader ReadInt64(ref Int64 value)
        {
            if (m_index < m_buffer.Length)
            {
                value = ReadInt64Variant();
            }

            return this;
        }

        public Reader ReadRepeatedInt64(ref List<Int64> value)
        {
            if (m_index < m_buffer.Length)
            {
                uint count = ReadUInt32Variant();
                if (count > 0)
                {
                    if (value == null)
                        value = new List<long>((int)count);

                    for (int i = 0; i < count; i++)
                    {
                        Int64 temp = ReadInt64Variant();
                        value.Add(temp);
                    }
                }
            }

            return this;
        }

        public Reader ReadUInt64(ref UInt64 value)
        {
            if (m_index < m_buffer.Length)
            {
                value = ReadUInt64Variant();
            }
            return this;
        }

        public Reader ReadRepeatedUInt64(ref List<UInt64> value)
        {
            if (m_index < m_buffer.Length)
            {
                uint count = ReadUInt32Variant();
                if (count > 0)
                {
                    if (value == null)
                        value = new List<ulong>((int)count);

                    for (int i = 0; i < count; i++)
                    {
                        UInt64 temp = ReadUInt64Variant();
                        value.Add(temp);
                    }
                }
            }

            return this;
        }

        public Reader ReadFloat(ref Single value)
        {
            if (m_index < m_buffer.Length)
            {
                value = m_binaryReader.ReadSingle();
                m_index += 4;
            }

            return this;
        }

        public Reader ReadRepeatedFloat(ref List<Single> value)
        {
            if (m_index < m_buffer.Length)
            {
                uint count = ReadUInt32Variant();
                if (count > 0)
                {
                    if (value == null)
                        value = new List<float>((int)count);

                    for (int i = 0; i < count; i++)
                    {
                        Single temp = m_binaryReader.ReadSingle();
                        m_index += 4;
                        value.Add(temp);
                    }
                }
            }

            return this;
        }

        public Reader ReadString(ref String value)
        {
            if (m_index < m_buffer.Length)
            {
                int length = 0;
                while (0 < m_buffer[m_index + length])
                    ++length;
                value = SerializeHelper.UTF8.GetString(m_buffer, m_index, length);
                m_index += length + 1;
                m_stream.Position += length + 1;
            }

            return this;
        }

        public Reader ReadRepeatedString(ref List<String> value)
        {
            if (m_index < m_buffer.Length)
            {
                uint count = ReadUInt32Variant();

                if (count > 0)
                {
                    value = new List<string>((int)count);
                    for (int i = 0; i < count; i++)
                    {
                        int length = 0;
                        while (0 < m_buffer[m_index + length])
                            ++length;
                        string temp = SerializeHelper.UTF8.GetString(m_buffer, m_index, length);
                        m_index += length + 1;
                        m_stream.Position += length + 1;
                        value.Add(temp);
                    }
                }
            }

            return this;
        }

        public Reader ReadItem<T>(uint key, ref T value) where T : Binary
        {
            if (m_index < m_buffer.Length)
            {
                if (value == null)
                    value = System.Activator.CreateInstance<T>();
                Reader reader = this;
                value.Read(ref reader);
                m_index++;
                m_stream.Position++;
            }

            return this;
        }

        public Reader ReadRepeatedItem<T>(uint key, ref List<T> value) where T : Binary
        {
            if (m_index < m_buffer.Length)
            {
                uint count = ReadUInt32Variant();

                if (count > 0)
                {
                    if (count > 0)
                    {
                        if (value == null)
                            value = new List<T>((int)count);

                        for (int i = 0; i < count; i++)
                        {
                            Binary temp = System.Activator.CreateInstance<T>();
                            Reader loader = this;
                            temp.Read(ref loader);
                            m_index++;
                            m_stream.Position++;
                            value.Add(temp as T);
                        }
                    }
                }
            }

            return this;
        }

        #region Variant

        public Int32 ReadInt32Variant()
        {
            return (Int32)ReadUInt32Variant();
        }

        public UInt32 ReadUInt32Variant()
        {
            UInt32 value = 0;
            byte tempByte = 0;
            int index = 0;
            do
            {
                tempByte = m_binaryReader.ReadByte();
                UInt32 temp1 = (UInt32)((tempByte & 0x7F) << index);  // 0x7F (1<<7)-1  127
                value |= temp1;
                index += 7;
                m_index++;
            }
            while ((tempByte >> 7) > 0);
            return value;
        }

        public Int64 ReadInt64Variant()
        {
            return (Int64)ReadUInt64Variant();
        }

        public UInt64 ReadUInt64Variant()
        {
            UInt64 value = 0;
            byte tempByte = 0;
            int index = 0;
            do
            {
                tempByte = m_binaryReader.ReadByte();
                UInt64 temp1 = (UInt64)((tempByte & 0x7F) << index);  // 0x7F (1<<7)-1  127
                value |= temp1;
                index += 7;
                m_index++;
            }
            while ((tempByte >> 7) > 0);
            return value;
        }

        #endregion

        public void Reset()
        {
            m_stream.SetLength(0);
            m_buffer = null;
            m_index = 0;
        }

        public void Close()
        {
            m_binaryReader.Close();
            m_stream.Close();
        }
    }
}
