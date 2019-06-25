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
            m_buffer = data;
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

        public Reader Read(ref bool value)
        {
            if (m_index < m_buffer.Length)
            {
                UInt32 temp = ReadUInt32Variant();
                value = temp == 1;
            }

            return this;
        }

        public Reader Read(ref List<bool> value)
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

        public Reader Read(ref Int32 value)
        {
            if (m_index < m_buffer.Length)
            {
                value = ReadInt32Variant();
            }

            return this;
        }

        public Reader Read(ref List<int> value)
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
                        int temp = ReadInt32Variant();
                        value.Add(temp);
                    }
                }
            }

            return this;
        }

        public Reader Read(ref uint value)
        {
            if (m_index < m_buffer.Length)
            {
                value = ReadUInt32Variant();
            }
            return this;
        }

        public Reader Read(ref List<uint> value)
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
                        uint temp = ReadUInt32Variant();
                        value.Add(temp);
                    }
                }
            }

            return this;
        }

        public Reader Read(ref Int64 value)
        {
            if (m_index < m_buffer.Length)
            {
                value = ReadInt64Variant();
            }

            return this;
        }

        public Reader Read(ref List<long> value)
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
                        long temp = ReadInt64Variant();
                        value.Add(temp);
                    }
                }
            }

            return this;
        }

        public Reader Read(ref ulong value)
        {
            if (m_index < m_buffer.Length)
            {
                value = ReadUInt64Variant();
            }
            return this;
        }

        public Reader Read(ref List<ulong> value)
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

        public Reader Read(ref float value)
        {
            if (m_index < m_buffer.Length)
            {
                value = m_binaryReader.ReadSingle();
                m_index += 4;
            }

            return this;
        }

        public Reader Read(ref List<float> value)
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
                        float temp = m_binaryReader.ReadSingle();
                        m_index += 4;
                        value.Add(temp);
                    }
                }
            }

            return this;
        }

        public Reader Read(ref double value)
        {
            if (m_index < m_buffer.Length)
            {
                value = m_binaryReader.ReadDouble();
                m_index += 8;
            }

            return this;
        }

        public Reader Read(ref List<double> value)
        {
            if (m_index < m_buffer.Length)
            {
                uint count = ReadUInt32Variant();
                if (count > 0)
                {
                    if (value == null)
                        value = new List<double>((int)count);

                    for (int i = 0; i < count; i++)
                    {
                        double temp = m_binaryReader.ReadDouble();
                        m_index += 8;
                        value.Add(temp);
                    }
                }
            }

            return this;
        }

        public Reader Read(ref string value)
        {
            if (m_index < m_buffer.Length)
            {
                int length = 0;
                while (0 < m_buffer[m_index + length])
                    ++length;
                value = Serializer.UTF8.GetString(m_buffer, m_index, length);
                m_index += length + 1;
                m_stream.Position += length + 1;
            }

            return this;
        }

        public Reader Read(ref List<string> value)
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
                        string temp = Serializer.UTF8.GetString(m_buffer, m_index, length);
                        m_index += length + 1;
                        m_stream.Position += length + 1;
                        value.Add(temp);
                    }
                }
            }

            return this;
        }

        public Reader Read<T>(ref T value) where T : Binary
        {
            if (m_index < m_buffer.Length)
            {
                int typeValue = ReadInt32Variant();
                Type type = Serializer.GetTypeByValue(typeValue);
                if (value == null)
                    value = System.Activator.CreateInstance(type) as T;
                Reader reader = this;
                value.Read(ref reader);
            }

            return this;
        }

        public Reader Read<T>(ref List<T> value) where T : Binary
        {
            if (m_index < m_buffer.Length)
            {
                uint count = ReadUInt32Variant();

                if (count > 0)
                {
                    if (value == null)
                        value = new List<T>((int)count);

                    for (int i = 0; i < count; i++)
                    {
                        int typeValue = ReadInt32Variant();
                        Type type = Serializer.GetTypeByValue(typeValue);
                        T temp = System.Activator.CreateInstance(type) as T;
                        Reader loader = this;
                        temp.Read(ref loader);
                        value.Add(temp as T);
                    }
                }
            }

            return this;
        }

        #region Variant

        public int ReadInt32Variant()
        {
            return (int)ReadUInt32Variant();
        }

        public uint ReadUInt32Variant()
        {
            uint value = 0;
            byte tempByte = 0;
            int index = 0;
            do
            {
                tempByte = m_binaryReader.ReadByte();
                uint temp1 = (uint)((tempByte & 0x7F) << index);  // 0x7F (1<<7)-1  127
                value |= temp1;
                index += 7;
                m_index++;
            }
            while ((tempByte >> 7) > 0);
            return value;
        }

        public long ReadInt64Variant()
        {
            return (long)ReadUInt64Variant();
        }

        public ulong ReadUInt64Variant()
        {
            ulong value = 0;
            byte tempByte = 0;
            int index = 0;
            do
            {
                tempByte = m_binaryReader.ReadByte();
                ulong temp1 = (ulong)((tempByte & 0x7F) << index);  // 0x7F (1<<7)-1  127
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
