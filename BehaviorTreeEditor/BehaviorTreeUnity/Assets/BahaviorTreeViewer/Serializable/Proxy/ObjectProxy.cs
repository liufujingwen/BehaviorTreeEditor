using System;
using System.Collections.Generic;
using Serializable.Def;
using Serializable.IO;

namespace Serializable.Proxy
{
    public class ObjectProxy : AbstractProxy
    {
        public ObjectProxy()
        {
        }

        public override object getValue(Context ctx, byte flag)
        {
            SerializableByteBuffer input = ctx.getBuffer();
            byte type = getFlagTypes(flag);
            if (type != Types.OBJECT)
            {
                throw new Exception("类型[" + Types.OBJECT + "], 不匹配[" + type + "]无效"); //WrongTypeException(Types.OBJECT, type);
            }

            byte signal = getFlagSignal(flag);
            if (signal == 0x00)
            {
                // #### 0000
                byte tag = input.readByte();
                int rawType = readVarInt32(input, tag);

                if (rawType == 0)
                {
                    // TODO 未定义的类型
                    throw new Exception("类型定义[" + rawType + "]无效"); //UnknowTypeDefException (rawType);
                }

                // 对象解析
                TypeDef def = ctx.getTypeDef(rawType);
                if (def == null || def.Code < 0)
                {
                    throw new Exception("类型定义[" + rawType + "]无效");// UnknowTypeDefException(rawType);
                }

                List<FieldDef> fields = def.Fields;
                // 对象赋值
                PO obj = def.newInstance();
                // 加入引用表
                ctx.putObjectRef(obj);
                // 字段数量, 最大255
                int len = 0xFF & input.readByte();
                for (int i = 0; i < len; i++)
                {
                    byte fValue = input.readByte();
                    FieldDef fieldDef = fields[i];
                    object value = ctx.getValue(fValue);
                    if (value == null)
                    {
                        continue;
                    }
                    // 字段赋值
                    fieldDef.setValue(obj, value);
                }
                return obj;
            }
            else if (signal == 0x01)
            {
                // #### 0001
                byte tag = input.readByte();
                int refIndex = readVarInt32(input, tag);
                Object result = ctx.getObjectRef(refIndex);
                return result;
            }
            throw new Exception("类型[" + type + "], 无效的标记[" + signal + "]无效"); //UnknowSignalException (type, signal);
        }

        public override void setValue(Context ctx, object value)
        {
            SerializableByteBuffer output = ctx.getBuffer();
            byte flag = Types.OBJECT;
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
                PO po = (PO)value;
                // 加入引用表
                ctx.putObjectRef(value);

                TypeDef def = ctx.getTypeDef(po.ObjectClass);
                if (def == null || def.Code < 0)
                {
                    // 类型定义不存在, 当作MAP处理
                    //throw new Exception(string.Format("不存在类型:{0},的定义", po.ObjectClass));

                    flag = Types.MAP;
                    MapProxy.setDictionary(ctx, po.dic, output, flag);
                }
                else
                {
                    // #### 0000
                    output.writeByte(flag);

                    int code = def.Code;
                    putVarInt32(output, code);

                    // 字段数量, 最大255
                    List<FieldDef> fields = def.Fields;
                    int size = fields.Count;
                    if (size > 0xFF)
                    {
                        throw new Exception("类型[" + po.ObjectClass + "]无效");// WrongTypeException(po.ObjectClass);
                    }
                    output.writeByte((byte)size);

                    foreach (var kv in po.dic)
                    {
                        if(!def.Contains(kv.Key))
                            throw new Exception("类型[" + po.ObjectClass + "]不存在[" + kv.Key + "]");// WrongTypeException(po.ObjectClass);
                    }

                    // 遍历属性
                    foreach (FieldDef fd in fields)
                    {
                        object o = fd.getValue(po);
                        ctx.setValue(o);
                    }
                }
            }

        }
    }
}

