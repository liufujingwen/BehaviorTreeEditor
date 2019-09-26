using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BehaviorTreeData
{
    public class Writer
    {
        MemoryStream m_stream = null;
        BinaryWriter m_binaryWriter = null;

        public Writer()
        {
            m_stream = new MemoryStream();
            m_binaryWriter = new BinaryWriter(m_stream);
        }

        public BinaryWriter writer
        {
            get { return m_binaryWriter; }
        }

        public MemoryStream stream
        {
            get { return m_stream; }
        }

        public int position
        {
            get { return (int)m_stream.Position; }
        }

        public byte[] GetBuffer()
        {
            return m_stream.ToArray();
        }

        public void Write(byte value)
        {
            m_binaryWriter.Write(value);
        }

        public Writer Write(bool value)
        {
            WriteUInt32Variant(value ? 1u : 0);
            return this;
        }

        public Writer Write(List<bool> value)
        {
            int count = value == null ? 0 : value.Count;
            WriteUInt32Variant((uint)count);

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    bool temp = value[i];
                    WriteUInt32Variant(temp ? 1u : 0);
                }
            }

            return this;
        }

        public Writer Write(string value)
        {
            value = value ?? string.Empty;
            value += "\0";
            m_binaryWriter.Write(Serializer.UTF8.GetBytes(value));
            return this;
        }

        public Writer Write(List<string> value)
        {
            int count = value == null ? 0 : value.Count;
            WriteUInt32Variant((uint)count);

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    string str = value[i];
                    if (str == null)
                        str = string.Empty;
                    str += "\0";

                    m_binaryWriter.Write(Serializer.UTF8.GetBytes(str));
                }
            }

            return this;
        }

        public Writer Write(int value)
        {
            WriteInt32Variant(value);
            return this;
        }

        public Writer Write(List<int> value)
        {
            int count = value == null ? 0 : value.Count;
            WriteUInt32Variant((uint)count);

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    int temp = value[i];
                    WriteInt32Variant(temp);
                }
            }

            return this;
        }

        public Writer Write(uint value)
        {
            WriteUInt32Variant(value);
            return this;
        }

        public Writer Write(List<uint> value)
        {
            int count = value == null ? 0 : value.Count;
            WriteUInt32Variant((uint)value.Count);

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    uint temp = value[i];
                    WriteUInt32Variant(temp);
                }
            }

            return this;
        }

        public Writer Write(long value)
        {
            WriteInt64Variant(value);
            return this;
        }

        public Writer Write(List<long> value)
        {
            int count = value == null ? 0 : value.Count;
            WriteUInt32Variant((uint)count);

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Int64 temp = value[i];
                    WriteInt64Variant(temp);
                }
            }

            return this;
        }

        public Writer Write(ulong value)
        {
            WriteUInt64Variant(value);
            return this;
        }

        public Writer Write(List<ulong> value)
        {
            int count = value == null ? 0 : value.Count;
            WriteUInt32Variant((uint)count);

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    UInt64 temp = value[i];
                    WriteUInt64Variant(temp);
                }
            }

            return this;
        }

        public Writer Write(float value)
        {
            m_binaryWriter.Write(value);
            return this;
        }

        public Writer Write(List<float> value)
        {
            int count = value == null ? 0 : value.Count;
            WriteUInt32Variant((uint)count);

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    float temp = value[i];
                    m_binaryWriter.Write(temp);
                }
            }

            return this;
        }

        public Writer Write(double value)
        {
            m_binaryWriter.Write(value);
            return this;
        }

        public Writer Write(List<double> value)
        {
            int count = value == null ? 0 : value.Count;
            WriteUInt32Variant((uint)count);

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    double temp = value[i];
                    m_binaryWriter.Write(temp);
                }
            }

            return this;
        }

        public Writer Write<T>(T value) where T : Binary
        {
            Writer writer = this;
            int typeValue = Serializer.GetValueByType(typeof(T));
            WriteInt32Variant(typeValue);
            value.Write(ref writer);
            return this;
        }

        public Writer Write<T>(List<T> value) where T : Binary
        {
            int count = value == null ? 0 : value.Count;
            WriteUInt32Variant((uint)count);

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Binary temp = value[i];
                    Writer writer = this;
                    Write(Serializer.GetValueByType(temp.GetType()));
                    temp.Write(ref writer);
                }
            }

            return this;
        }

        #region Variant

        public void WriteInt32Variant(int value)
        {
            //如果是负数需要补码
            uint temp = value < 0 ? (uint)(~Math.Abs(value) + 1) : (uint)value;
            WriteUInt32Variant(temp);
        }

        public void WriteUInt32Variant(uint value)
        {
            do
            {
                //取7个字节
                uint temp = (uint)(value & 0x7F);//0x7F = ((1 << 7) - 1) = 127

                value >>= 7;

                if (value > 0)
                    temp |= 0x80;//最高位补1    128

                m_binaryWriter.Write((byte)temp);
            }
            while (value != 0);
        }

        public void WriteInt64Variant(long value)
        {
            //如果是负数需要补码
            ulong temp = value < 0 ? (ulong)(~Math.Abs(value) + 1) : (ulong)value;
            WriteUInt64Variant(temp);
        }

        public void WriteUInt64Variant(ulong value)
        {
            do
            {
                //取7个字节
                ulong temp = (ulong)(value & 0x7F);//0x7F = ((1 << 7) - 1) = 127

                value >>= 7;

                if (value > 0)
                    temp |= 0x80;//最高位补1    128

                m_binaryWriter.Write((byte)temp);
            }
            while (value != 0);
        }

        #endregion

        public void Close()
        {
            m_stream.Close();
            m_binaryWriter.Close();
        }
    }
}
