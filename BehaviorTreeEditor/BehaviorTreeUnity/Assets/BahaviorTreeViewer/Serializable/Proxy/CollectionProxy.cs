using System;
using System.Collections;
using System.Collections.Generic;
using Serializable.IO;

namespace Serializable.Proxy
{
    public class CollectionProxy : AbstractProxy
    {
        public CollectionProxy()
        {
        }

        public override object getValue(Context ctx, byte flag)
        {
            byte type = getFlagTypes(flag);
            if (type != Types.COLLECTION)
            {
                throw new Exception("类型[" + Types.COLLECTION + "], 不匹配[" + type + "]无效"); // WrongTypeException(Types.COLLECTION, type);
            }
            byte signal = getFlagSignal(flag);

            // 读取数组
            byte arrayFlag = (byte)(Types.ARRAY | signal);
            List<object> array = (List<object>)ctx.getValue(arrayFlag);
            return new ArrayList(array);
        }

        public override void setValue(Context ctx, object value)
        {
            SerializableByteBuffer output = ctx.getBuffer();
            byte flag = Types.COLLECTION;
            int refIndex = ctx.getObjectRef(value);
            if (refIndex > 0)
            {
                // #### 0001
                flag |= 0x01;
                output.writeByte(flag);
                putVarInt32(output, refIndex);
            }
            else
            {
                // 加入引用表
                ctx.putObjectRef(value);

                // #### 0000
                output.writeByte(flag);
                ArrayList array = new ArrayList((ICollection)value);

                int len = array.Count;
                putVarInt32(output, len);
                for (int i = 0; i < len; i++)
                {
                    Object obj = array[i];
                    ctx.setValue(obj);
                }
            }
        }
    }
}

