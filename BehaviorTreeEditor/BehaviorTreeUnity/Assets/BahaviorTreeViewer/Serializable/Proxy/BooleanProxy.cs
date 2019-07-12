using Serializable.IO;
using System;
using System.IO;

namespace Serializable.Proxy
{
    public class BooleanProxy : AbstractProxy
    {
        public BooleanProxy()
        {
        }

        public override object getValue(Context ctx, byte flag)
        {
            // ByteBuffer in = ctx.getBuffer();
            byte type = getFlagTypes(flag);
            if (type != Types.BOOLEAN)
            {
                throw new Exception("类型[" + Types.BOOLEAN + "], 不匹配[" + type + "]无效"); //WrongTypeException(Types.BOOLEAN, type);
            }

            byte signal = getFlagSignal(flag);
            if (signal == 0x00)
            {
                return false;
            }
            else if (signal == 0x01)
            {
                return true;
            }
            throw new Exception("BOOLEAN 类型[" + type + "], 无效的标记[" + signal + "]无效");  //UnknowSignalException (type, signal);
        }

        public override void setValue(Context ctx, object value)
        {
            SerializableByteBuffer output = ctx.getBuffer();
            byte flag = Types.BOOLEAN;

            if ((bool)value)
            {
                // #### 0001
                flag |= 0x01;
                output.writeByte(flag);
            }
            else
            {
                // #### 0000
                output.writeByte(flag);
            }

        }
    }
}

