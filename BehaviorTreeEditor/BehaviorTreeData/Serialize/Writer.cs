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

        public Writer WriteBoolean(uint key, Boolean value, Boolean defaultValue)
        {
            if (value != defaultValue)
            {
                WriteKey(key, value != defaultValue);
                WriteUInt32Variant(value ? 1u : 0);
            }
            return this;
        }

        public Writer WriteRepeatedBoolean(uint key, List<Boolean> value)
        {
            if (value.Count > 0)
            {
                WriteKey(key, value.Count > 0);
                WriteUInt32Variant((uint)value.Count);
                for (int i = 0; i < value.Count; i++)
                {
                    Boolean temp = value[i];
                    WriteUInt32Variant(temp ? 1u : 0);
                }
            }

            return this;
        }

        public Writer WriteEnum(uint key, Int32 value, Int32 defaultValue)
        {
            if (value != defaultValue)
            {
                WriteKey(key, value != defaultValue);
                WriteInt32Variant(value);
            }
            return this;
        }

        public Writer WriteString(uint key, String value, String defaultValue)
        {
            bool hasValule = value != null && value != defaultValue;

            if (hasValule)
            {
                WriteKey(key, hasValule);
                value += "\0";
                m_binaryWriter.Write(SerializeHelper.UTF8.GetBytes(value));
            }
            return this;
        }

        public Writer WriteRepeatedString(uint key, List<String> value)
        {
            if (value.Count > 0)
            {
                WriteKey(key, value.Count > 0);
                WriteUInt32Variant((uint)value.Count);
                for (int i = 0; i < value.Count; i++)
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

        public Writer WriteInt32(uint key, Int32 value, Int32 defaultValue)
        {
            if (value != defaultValue)
            {
                WriteKey(key, value != defaultValue);
                WriteInt32Variant(value);
            }
            return this;
        }

        public Writer WriteRepeatedInt32(uint key, List<Int32> value)
        {
            if (value.Count > 0)
            {
                WriteKey(key, value.Count > 0);
                WriteUInt32Variant((uint)value.Count);
                for (int i = 0; i < value.Count; i++)
                {
                    Int32 temp = value[i];
                    WriteInt32Variant(temp);
                }
            }

            return this;
        }

        public Writer WriteUInt32(uint key, UInt32 value, UInt32 defaultValue)
        {
            if (value != defaultValue)
            {
                WriteKey(key, value != defaultValue);
                WriteUInt32Variant(value);
            }
            return this;
        }

        public Writer WriteRepeatedUInt32(uint key, List<UInt32> value)
        {
            if (value.Count > 0)
            {
                WriteKey(key, value.Count > 0);
                WriteUInt32Variant((uint)value.Count);
                for (int i = 0; i < value.Count; i++)
                {
                    UInt32 temp = value[i];
                    WriteUInt32Variant(temp);
                }
            }

            return this;
        }

        public Writer WriteInt64(uint key, Int64 value, Int64 defaultValue)
        {
            if (value != defaultValue)
            {
                WriteKey(key, value != defaultValue);
                WriteInt64Variant(value);
            }
            return this;
        }

        public Writer WriteRepeatedInt64(uint key, List<Int64> value)
        {
            if (value.Count > 0)
            {
                WriteKey(key, value.Count > 0);
                WriteUInt32Variant((uint)value.Count);
                for (int i = 0; i < value.Count; i++)
                {
                    Int64 temp = value[i];
                    WriteInt64Variant(temp);
                }
            }

            return this;
        }

        public Writer WriteUInt64(uint key, UInt64 value, UInt64 defaultValue)
        {
            if (value != defaultValue)
            {
                WriteKey(key, value != defaultValue);
                WriteUInt64Variant(value);
            }
            return this;
        }

        public Writer WriteRepeatedUInt64(uint key, List<UInt64> value)
        {
            if (value.Count > 0)
            {
                WriteKey(key, value.Count > 0);
                WriteUInt32Variant((uint)value.Count);
                for (int i = 0; i < value.Count; i++)
                {
                    UInt64 temp = value[i];
                    WriteUInt64Variant(temp);
                }
            }

            return this;
        }

        public Writer WriteFloat(uint key, Single value, Single defaultValue)
        {
            if (value != defaultValue)
            {
                WriteKey(key, value != defaultValue);
                m_binaryWriter.Write(value);
            }
            return this;
        }

        public Writer WriteRepeatedFloat(uint key, List<Single> value)
        {
            if (value.Count > 0)
            {
                WriteKey(key, value.Count > 0);
                WriteUInt32Variant((uint)value.Count);
                for (int i = 0; i < value.Count; i++)
                {
                    Single temp = value[i];
                    m_binaryWriter.Write(temp);
                }
            }

            return this;
        }

        public Writer WriteItem<T>(uint key, T value) where T : Binary
        {
            if (value != null)
            {
                WriteKey(key, value != null);
                Writer writer = this;
                value.Write(ref writer);
                writer.m_binaryWriter.Write(SerializeHelper.MESSAGE_END_FLAG);
            }
            return this;
        }

        public Writer WriteRepeatedItem<T>(uint key, List<T> value) where T : Binary
        {
            if (value.Count > 0)
            {
                WriteKey(key, value.Count > 0);
                WriteUInt32Variant((uint)value.Count);
                for (int i = 0; i < value.Count; i++)
                {
                    Binary temp = value[i];
                    Writer writer = this;
                    temp.Write(ref writer);
                    writer.m_binaryWriter.Write(SerializeHelper.MESSAGE_END_FLAG);
                }
            }
            return this;
        }

        void WriteKey(ulong key, bool hasValue)
        {
            key = key << 3 | (byte)(hasValue ? 1 : 0);
            WriteUInt64Variant(key);
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
