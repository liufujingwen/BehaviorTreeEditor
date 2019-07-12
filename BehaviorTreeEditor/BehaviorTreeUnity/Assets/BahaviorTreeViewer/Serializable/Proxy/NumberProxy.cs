using Serializable.IO;
using System;

namespace Serializable.Proxy
{
    public class NumberProxy : AbstractProxy
    {

        public const byte INT32 = 0x01;
        public const byte INT64 = 0x02;
        public const byte FLOAT = 0x03;
        public const byte DOUBLE = 0x04;

        // 0000 1000
        public const int FLAG_NEVIGATE = 0x08;


        public NumberProxy()
        {
        }

        public override object getValue(Context ctx, byte flag)
        {
            SerializableByteBuffer input = ctx.getBuffer();
            byte type = getFlagTypes(flag);
            if (type != Types.NUMBER)
            {
                throw new Exception("类型[" + Types.NUMBER + "], 不匹配[" + type + "]无效"); //WrongTypeException (Types.NUMBER, type);
            }

            // 0000 #000
            bool nevigate = ((flag & FLAG_NEVIGATE) != 0);

            // 0000 0###
            byte signal = getFlagSignal(flag);
            if (signal == INT32)
            {
                byte tag = input.readByte();
                int value = readVarInt32(input, tag);
                return nevigate ? (value == 0 ? int.MaxValue : -value) : value;
            }
            else if (signal == INT64)
            {
                byte tag = input.readByte();
                long value = readVarInt64(input, tag);
                return nevigate ? (value == 0 ? long.MaxValue : -value) : value;
            }
            else if (signal == FLOAT)
            {
                float value = input.readFloat();
                return value;
            }
            else if (signal == DOUBLE)
            {
                double value = input.readDouble();
                return value;
            }
            throw new Exception("类型[" + type + "], 无效的标记[" + signal + "]无效"); //UnknowSignalException (type, signal);
        }

        public override void setValue(Context ctx, object value)
        {
            SerializableByteBuffer output = ctx.getBuffer();
            byte flag = Types.NUMBER;
            if (value is byte || value is short || value is ushort || value is int || value is uint)
            {
                int v;
                if (value is uint && Convert.ToUInt32(value) > int.MaxValue)
                {
                    flag |= FLAG_NEVIGATE | INT32;
                    ulong v1 = Convert.ToUInt32(value);
                    ulong v2 = Convert.ToUInt32(int.MaxValue);
                    ulong dif = (v2 + 1) + (v2 + 1 - v1);
                    v = Convert.ToInt32(dif);
                }
                else
                {
                    v = Convert.ToInt32(value);
                    if (v < 0)
                    {
                        flag |= FLAG_NEVIGATE | INT32;
                        if (v == int.MinValue)
                        {
                            v = 0;
                        }
                        else
                        {
                            v = -v;
                        }
                    }
                    else
                    {
                        flag |= INT32;
                    }
                }
                output.writeByte(flag);
                putVarInt32(output, v);
            }
            else if (value is long || value is ulong)
            {
                long v;
                if (value is ulong && Convert.ToUInt64(value) > long.MaxValue)
                {
                    flag |= FLAG_NEVIGATE | INT64;
                    ulong v1 = Convert.ToUInt64(value);
                    ulong v2 = Convert.ToUInt64(long.MaxValue);
                    ulong dif = (v2 + 1) + (v2 + 1 - v1);
                    v = Convert.ToInt64(dif);
                }
                else
                {
                    v = Convert.ToInt64(value);
                    if (v < 0)
                    {
                        flag |= FLAG_NEVIGATE | INT64;
                        if (v == long.MinValue)
                        {
                            v = 0;
                        }
                        else
                        {
                            v = -v;
                        }
                    }
                    else
                    {
                        flag |= INT64;
                    }
                }
                output.writeByte(flag);
                putVarInt64(output, v);
            }
            else if (value is float)
            {
                float v = (float)value;
                flag |= FLOAT;
                output.writeByte(flag);
                output.writeFloat(v);
            }
            else if (value is double)
            {
                double v = (double)value;
                flag |= DOUBLE;
                output.writeByte(flag);
                output.writeDouble(v);
            }
            else
            {
                new Exception("无法识别的Number类型:" + value.GetType());
            }
        }
    }
}

