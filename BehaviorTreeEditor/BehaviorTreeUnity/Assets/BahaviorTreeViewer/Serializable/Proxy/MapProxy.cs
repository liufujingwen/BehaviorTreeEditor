using System;
using System.Collections;
using System.Collections.Generic;
using Serializable.IO;

namespace Serializable.Proxy
{
    public class MapProxy : AbstractProxy
    {
        public MapProxy()
        {
        }

        public override object getValue(Context ctx, byte flag)
        {
            // 非明确定义的对象类型，均当做MAP解析

            SerializableByteBuffer input = ctx.getBuffer();
            byte type = getFlagTypes(flag);
            if (type != Types.MAP)
            {
                throw new Exception("类型[" + Types.MAP + "], 不匹配[" + type + "]无效");  //WrongTypeException(Types.MAP, type);
            }

            byte signal = getFlagSignal(flag);
            if (signal == 0x00)
            {
                // 对象解析
                //try
                {
                    // 对象赋值
                    IDictionary result = new Dictionary<object, object>();
                    // 加入引用表
                    ctx.putObjectRef(result);
                    // 字段数量
                    byte tag = input.readByte();
                    int len = readVarInt32(input, tag);
                    for (int i = 0; i < len; i++)
                    {
                        byte fKey = input.readByte();
                        Object key = ctx.getValue(fKey);
                        byte fValue = input.readByte();
                        Object value = ctx.getValue(fValue);
                        // 字段赋值
                        result.Add(key, value);
                    }
                    return result;
                }
                //catch (Exception e)
                //{
                //    throw e;
                //}
            }
            else if (signal == 0x01)
            {
                // #### 0001
                byte tag = input.readByte();
                int refIndex = readVarInt32(input, tag);
                IDictionary result = (IDictionary)ctx.getObjectRef(refIndex);
                return result;
            }

            throw new Exception("类型[" + type + "], 无效的标记[" + signal + "]无效"); //UnknowSignalException (type, signal);
        }

        public override void setValue(Context ctx, object value)
        {
            SerializableByteBuffer output = ctx.getBuffer();
            byte flag = Types.MAP;
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
                IDictionary dict = (IDictionary)value;
                setDictionary(ctx, dict, output, flag);
            }
        }

        internal static void setDictionary(Context ctx, IDictionary dict, SerializableByteBuffer output, byte flag)
        {
            // #### 0000
            output.writeByte(flag);

            // 字段数量
            int size = dict.Count;
            putVarInt32(output, size);
            foreach (var e in dict.Keys)
            {
                ctx.setValue(e);
                object v = dict[e];
                ctx.setValue(v);
            }
        }
    }
}

