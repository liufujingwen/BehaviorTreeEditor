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

        public void WriteByte(byte value)
        {
            m_binaryWriter.Write(value);
        }

        public void WriteUInt16(UInt16 value)
        {
            m_binaryWriter.Write(value);
        }

        public Writer WriteBoolean(Boolean value)
        {
            WriteUInt32Variant(value ? 1u : 0);
            return this;
        }

        public Writer WriteRepeatedBoolean(List<Boolean> value)
        {
            int count = value == null ? 0 : value.Count;
            WriteUInt32Variant((uint)count);

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Boolean temp = value[i];
                    WriteUInt32Variant(temp ? 1u : 0);
                }
            }

            return this;
        }

        public Writer WriteEnum(ref Int32 value)
        {
            WriteInt32Variant(value);
            return this;
        }

        public Writer WriteString(ref String value)
        {
            value = value ?? string.Empty;
            value += "\0";
            m_binaryWriter.Write(SerializeHelper.UTF8.GetBytes(value));
            return this;
        }

        public Writer WriteRepeatedString(ref List<String> value)
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

                    m_binaryWriter.Write(SerializeHelper.UTF8.GetBytes(str));
                }
            }

            return this;
        }

        public Writer WriteInt32(ref Int32 value)
        {
            WriteInt32Variant(value);
            return this;
        }

        public Writer WriteRepeatedInt32(ref List<Int32> value)
        {
            int count = value == null ? 0 : value.Count;
            WriteUInt32Variant((uint)count);

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Int32 temp = value[i];
                    WriteInt32Variant(temp);
                }
            }

            return this;
        }

        public Writer WriteUInt32(ref UInt32 value)
        {
            WriteUInt32Variant(value);
            return this;
        }

        public Writer WriteRepeatedUInt32(ref List<UInt32> value)
        {
            int count = value == null ? 0 : value.Count;
            WriteUInt32Variant((uint)value.Count);

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    UInt32 temp = value[i];
                    WriteUInt32Variant(temp);
                }
            }

            return this;
        }

        public Writer WriteInt64(ref Int64 value)
        {
            WriteInt64Variant(value);
            return this;
        }

        public Writer WriteRepeatedInt64(ref List<Int64> value)
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

        public Writer WriteUInt64(ref UInt64 value)
        {
            WriteUInt64Variant(value);
            return this;
        }

        public Writer WriteRepeatedUInt64(ref List<UInt64> value)
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

        public Writer WriteFloat(ref Single value)
        {
            m_binaryWriter.Write(value);
            return this;
        }

        public Writer WriteRepeatedFloat(ref List<Single> value)
        {
            int count = value == null ? 0 : value.Count;
            WriteUInt32Variant((uint)count);

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Single temp = value[i];
                    m_binaryWriter.Write(temp);
                }
            }

            return this;
        }

        public Writer WriteItem<T>(ref T value) where T : Binary
        {
            Writer writer = this;
            value.Write(ref writer);
            return this;
        }

        public Writer WriteRepeatedItem<T>(uint key, List<T> value) where T : Binary
        {
            int count = value == null ? 0 : value.Count;
            WriteUInt32Variant((uint)count);

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Binary temp = value[i];
                    Writer writer = this;
                    temp.Write(ref writer);
                }
            }

            return this;
        }

        #region Variant

        public void WriteInt32Variant(Int32 value)
        {
            //如果是负数需要补码
            UInt32 temp = value < 0 ? (UInt32)(~Math.Abs(value) + 1) : (UInt32)value;
            WriteUInt32Variant(temp);
        }

        public void WriteUInt32Variant(UInt32 value)
        {
            do
            {
                //取7个字节
                UInt32 temp = (UInt32)(value & 0x7F);//0x7F = ((1 << 7) - 1) = 127

                value >>= 7;

                if (value > 0)
                    temp |= 0x80;//最高位补1    128

                m_binaryWriter.Write((byte)temp);
            }
            while (value != 0);
        }

        public void WriteInt64Variant(Int64 value)
        {
            //如果是负数需要补码
            UInt64 temp = value < 0 ? (UInt64)(~Math.Abs(value) + 1) : (UInt64)value;
            WriteUInt64Variant(temp);
        }

        public void WriteUInt64Variant(UInt64 value)
        {
            do
            {
                //取7个字节
                UInt64 temp = (UInt64)(value & 0x7F);//0x7F = ((1 << 7) - 1) = 127

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
