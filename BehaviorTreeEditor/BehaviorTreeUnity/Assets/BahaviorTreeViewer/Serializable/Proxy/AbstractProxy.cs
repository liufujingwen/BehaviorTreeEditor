using System;
using System.IO;
using Serializable.IO;

namespace Serializable.Proxy
{
    public abstract class AbstractProxy : IProxy
    {


        // 1111 0000
        public const byte TYPE_MASK = (byte)0xF0;

        // 0000 0111
        public const byte SIGNAL_MASK = (byte)0x07;

        // 0000 1111
        public const byte NUMBER_BITS = (byte)0x0F;

        // 1000 0000
        public const int NUMBER_FLAGS = 0x80;

        /**
	 * Types
	 * @return #### 0000
	 */
        public static byte getFlagTypes(byte flag)
        {
            byte code = (byte)(flag & TYPE_MASK);
            if (code == 0)
            {
                return flag;
            }
            return code;
        }

        /**
	 * Signal
	 * @return 0000 0###
	 */
        public static byte getFlagSignal(byte flag)
        {
            byte signal = (byte)(flag & SIGNAL_MASK);
            return signal;
        }

        public static int readVarInt32(SerializableByteBuffer input, byte tag)
        {
            // 1### #### (128 - (byte)0x80)
            if ((tag & NUMBER_FLAGS) == 0)
            {
                return tag & 0x7F;
            }

            int signal = tag & NUMBER_BITS;
            if (input.Remaining < signal)
            {
                throw new EndOfStreamException();
            }

            if (signal > 4 || signal < 0)
            {
                throw new Exception("最大的变长整数位数[" + 4 + "], 当前位数[" + signal + "]无效"); //MalformedVarintException (4, signal);
            }

            int result = 0;
            for (int i = 8 * (signal - 1); i >= 0; i -= 8)
            {
                byte b = input.readByte();
                result |= (b & 0xFF) << i;
            }
            return result;
        }

        public static void putVarInt32(SerializableByteBuffer output, int value)
        {
            if (value < 0)
            {
                // 不能 < 0
                throw new Exception("无效的变长整数值[" + value + "]"); //new MalformedVarintException(value);
            }

            uint unsign = (uint)value;
            // 1### #### (128 - (byte)0x80)
            if (value < NUMBER_FLAGS)
            {
                byte b = (byte)value;
                output.writeByte(b);
            }
            else if (value <= int.MaxValue)
            {
                // VarInt32
                if ((unsign >> 24) > 0)
                {
                    byte b = (byte)(NUMBER_FLAGS | 4);
                    output.writeByte(b);
                    //
                    byte b1 = (byte)(unsign >> 24 & 0xFF);
                    byte b2 = (byte)(unsign >> 16 & 0xFF);
                    byte b3 = (byte)(unsign >> 8 & 0xFF);
                    byte b4 = (byte)(value & 0xFF);
                    output.writeByte(b1);
                    output.writeByte(b2);
                    output.writeByte(b3);
                    output.writeByte(b4);
                }
                else if ((unsign >> 16) > 0)
                {
                    byte b = (byte)(NUMBER_FLAGS | 3);
                    output.writeByte(b);
                    //
                    byte b2 = (byte)(unsign >> 16 & 0xFF);
                    byte b3 = (byte)(unsign >> 8 & 0xFF);
                    byte b4 = (byte)(value & 0xFF);
                    output.writeByte(b2);
                    output.writeByte(b3);
                    output.writeByte(b4);
                }
                else if ((unsign >> 8) > 0)
                {
                    byte b = (byte)(NUMBER_FLAGS | 2);
                    output.writeByte(b);
                    //
                    byte b3 = (byte)(unsign >> 8 & 0xFF);
                    byte b4 = (byte)(value & 0xFF);
                    output.writeByte(b3);
                    output.writeByte(b4);
                }
                else
                {
                    byte b = (byte)(NUMBER_FLAGS | 1);
                    output.writeByte(b);
                    //
                    byte b4 = (byte)(value & 0xFF);
                    output.writeByte(b4);
                }
            }
            else
            {
                // 不支持
                throw new ArgumentException("VarInt值超过范围");
            }
        }

        public static long readVarInt64(SerializableByteBuffer input, byte tag)
        {
            // 1### #### (128 - (byte)0x80)
            if ((tag & NUMBER_FLAGS) == 0)
            {
                return tag & 0x7F;
            }

            int signal = tag & NUMBER_BITS;
            if (input.Remaining < signal)
            {
                throw new EndOfStreamException();
            }

            if (signal > 8 || signal < 0)
            {
                throw new Exception("最大的变长整数位数[" + 8 + "], 当前位数[" + signal + "]无效"); //MalformedVarintException(8, signal);
            }

            long result = 0;
            for (int i = 8 * (signal - 1); i >= 0; i -= 8)
            {
                byte b = input.readByte();
                result |= (long)(b & 0xFF) << i;
            }
            return result;
        }

        public static void putVarInt64(SerializableByteBuffer output, long value)
        {
            if (value < 0)
            {
                // 不能 < 0
                throw new Exception("无效的变长整数值[" + value + "]"); //MalformedVarintException(value);
            }

            ulong unsign = (ulong)value;
            // 1### #### (128 - (byte)0x80)
            if (value < NUMBER_FLAGS)
            {
                byte b = (byte)value;
                output.writeByte(b);
            }
            else if (value <= int.MaxValue)
            {
                // VarInt32
                if ((unsign >> 24) > 0)
                {
                    byte b = (byte)(NUMBER_FLAGS | 4);
                    output.writeByte(b);
                    //
                    byte b1 = (byte)(unsign >> 24 & 0xFF);
                    byte b2 = (byte)(unsign >> 16 & 0xFF);
                    byte b3 = (byte)(unsign >> 8 & 0xFF);
                    byte b4 = (byte)(value & 0xFF);
                    output.writeByte(b1);
                    output.writeByte(b2);
                    output.writeByte(b3);
                    output.writeByte(b4);
                }
                else if ((unsign >> 16) > 0)
                {
                    byte b = (byte)(NUMBER_FLAGS | 3);
                    output.writeByte(b);
                    //
                    byte b2 = (byte)(unsign >> 16 & 0xFF);
                    byte b3 = (byte)(unsign >> 8 & 0xFF);
                    byte b4 = (byte)(value & 0xFF);
                    output.writeByte(b2);
                    output.writeByte(b3);
                    output.writeByte(b4);
                }
                else if ((unsign >> 8) > 0)
                {
                    byte b = (byte)(NUMBER_FLAGS | 2);
                    output.writeByte(b);
                    //
                    byte b3 = (byte)(unsign >> 8 & 0xFF);
                    byte b4 = (byte)(value & 0xFF);
                    output.writeByte(b3);
                    output.writeByte(b4);
                }
                else
                {
                    byte b = (byte)(NUMBER_FLAGS | 1);
                    output.writeByte(b);
                    //
                    byte b4 = (byte)(value & 0xFF);
                    output.writeByte(b4);
                }
            }
            else if (value <= long.MaxValue)
            {
                // VarInt64
                if ((unsign >> 56) > 0)
                {
                    byte b = (byte)(NUMBER_FLAGS | 8);
                    output.writeByte(b);
                    //
                    byte b0 = (byte)(unsign >> 56 & 0xFF);
                    byte b1 = (byte)(unsign >> 48 & 0xFF);
                    byte b2 = (byte)(unsign >> 40 & 0xFF);
                    byte b3 = (byte)(unsign >> 32 & 0xFF);
                    byte b4 = (byte)(unsign >> 24 & 0xFF);
                    byte b5 = (byte)(unsign >> 16 & 0xFF);
                    byte b6 = (byte)(unsign >> 8 & 0xFF);
                    byte b7 = (byte)(value & 0xFF);
                    output.writeByte(b0);
                    output.writeByte(b1);
                    output.writeByte(b2);
                    output.writeByte(b3);
                    output.writeByte(b4);
                    output.writeByte(b5);
                    output.writeByte(b6);
                    output.writeByte(b7);
                }
                else if ((unsign >> 48) > 0)
                {
                    byte b = (byte)(NUMBER_FLAGS | 7);
                    output.writeByte(b);
                    //
                    byte b1 = (byte)(unsign >> 48 & 0xFF);
                    byte b2 = (byte)(unsign >> 40 & 0xFF);
                    byte b3 = (byte)(unsign >> 32 & 0xFF);
                    byte b4 = (byte)(unsign >> 24 & 0xFF);
                    byte b5 = (byte)(unsign >> 16 & 0xFF);
                    byte b6 = (byte)(unsign >> 8 & 0xFF);
                    byte b7 = (byte)(value & 0xFF);
                    output.writeByte(b1);
                    output.writeByte(b2);
                    output.writeByte(b3);
                    output.writeByte(b4);
                    output.writeByte(b5);
                    output.writeByte(b6);
                    output.writeByte(b7);
                }
                else if ((unsign >> 40) > 0)
                {
                    byte b = (byte)(NUMBER_FLAGS | 6);
                    output.writeByte(b);
                    //
                    byte b2 = (byte)(unsign >> 40 & 0xFF);
                    byte b3 = (byte)(unsign >> 32 & 0xFF);
                    byte b4 = (byte)(unsign >> 24 & 0xFF);
                    byte b5 = (byte)(unsign >> 16 & 0xFF);
                    byte b6 = (byte)(unsign >> 8 & 0xFF);
                    byte b7 = (byte)(value & 0xFF);
                    output.writeByte(b2);
                    output.writeByte(b3);
                    output.writeByte(b4);
                    output.writeByte(b5);
                    output.writeByte(b6);
                    output.writeByte(b7);
                }
                else if ((unsign >> 32) > 0)
                {
                    byte b = (byte)(NUMBER_FLAGS | 5);
                    output.writeByte(b);
                    //
                    byte b3 = (byte)(unsign >> 32 & 0xFF);
                    byte b4 = (byte)(unsign >> 24 & 0xFF);
                    byte b5 = (byte)(unsign >> 16 & 0xFF);
                    byte b6 = (byte)(unsign >> 8 & 0xFF);
                    byte b7 = (byte)(value & 0xFF);
                    output.writeByte(b3);
                    output.writeByte(b4);
                    output.writeByte(b5);
                    output.writeByte(b6);
                    output.writeByte(b7);
                }
                else
                {
                    byte b = (byte)(NUMBER_FLAGS | 4);
                    output.writeByte(b);
                    //
                    byte b4 = (byte)(unsign >> 24 & 0xFF);
                    byte b5 = (byte)(unsign >> 16 & 0xFF);
                    byte b6 = (byte)(unsign >> 8 & 0xFF);
                    byte b7 = (byte)(value & 0xFF);
                    output.writeByte(b4);
                    output.writeByte(b5);
                    output.writeByte(b6);
                    output.writeByte(b7);
                }
            }
            else
            {
                // 不支持
                throw new ArgumentException("VarInt值超过范围");
            }
        }

        public abstract object getValue(Context ctx, byte flag);

        public abstract void setValue(Context ctx, object value);

    }
}

