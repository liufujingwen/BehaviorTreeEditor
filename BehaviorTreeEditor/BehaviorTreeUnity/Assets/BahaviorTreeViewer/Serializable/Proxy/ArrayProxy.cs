using Serializable.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Serializable.Proxy
{
    public class ArrayProxy : AbstractProxy
    {
        public ArrayProxy()
        {
        }

        public override object getValue(Context ctx, byte flag)
        {
            SerializableByteBuffer input = ctx.getBuffer();
            byte type = getFlagTypes(flag);
            if (type != Types.ARRAY)
            {
                throw new Exception("类型[" + Types.ARRAY + "], 不匹配[" + type + "]无效"); // WrongTypeException(Types.ARRAY, type);
            }

            byte signal = getFlagSignal(flag);
            if (signal == 0x00)
            {
                // #### 0000
                byte tag = input.readByte();
                int len = readVarInt32(input, tag);
                if (input.Remaining < len)
                {
                    throw new EndOfStreamException();
                }
                //Object[] result = new Object[len];
                //// 加入引用表
                //ctx.putObjectRef(result);
                //for (int i = 0; i < len; i++)
                //{
                //    byte fValue = input.readByte();
                //    Object value = ctx.getValue(fValue);
                //    result[i] = value;
                //}

                List<Object> result = new List<Object>(len);
                // 加入引用表
                ctx.putObjectRef(result);
                for (int i = 0; i < len; i++)
                {
                    byte fValue = input.readByte();
                    Object value = ctx.getValue(fValue);
                    result.Add(value);
                }

                return result;
            }
            else if (signal == 0x01)
            {
                // #### 0001
                byte tag = input.readByte();
                int refIndex = readVarInt32(input, tag);
                //object[] result = (object[])ctx.getObjectRef(refIndex);
                List<Object> result = (List<Object>)ctx.getObjectRef(refIndex);
                return result;
            }
            throw new Exception("类型[" + type + "], 无效的标记[" + signal + "]无效"); //UnknowSignalException(type, signal);
        }

        public override void setValue(Context ctx, object value)
        {
            SerializableByteBuffer output = ctx.getBuffer();
            byte flag = Types.ARRAY;
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
                //Array array = (Array)value;
                //int len = array.Length;
                //putVarInt32(output, len);
                //for (int i = 0; i < len; i++)
                //{
                //    object obj = array.GetValue(i);
                //    ctx.setValue(obj);
                //}

                IList array = (IList)value;
                int len = array.Count;
                putVarInt32(output, len);
                for (int i = 0; i < len; i++)
                {
                    object obj = array[i];
                    ctx.setValue(obj);
                }
            }
        }
    }
}

