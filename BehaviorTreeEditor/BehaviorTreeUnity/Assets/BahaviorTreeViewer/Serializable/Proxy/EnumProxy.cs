using System;
using Serializable.IO;

namespace Serializable.Proxy
{
    public class EnumProxy : AbstractProxy
    {
        public EnumProxy()
        {
        }

        public override object getValue(Context ctx, byte flag)
        {
            SerializableByteBuffer input = ctx.getBuffer();
            byte type = getFlagTypes(flag);
            if (type != Types.ENUM)
            {
                throw new Exception("类型[" + Types.ENUM + "], 不匹配[" + type + "]无效"); //WrongTypeException(Types.ENUM, type);
            }

            byte tag = input.readByte();
            int enumValue = readVarInt32(input, tag);
            return enumValue;
        }

        public override void setValue(Context ctx, object value)
        {
            SerializableByteBuffer output = ctx.getBuffer();
            byte flag = Types.ENUM;
            int enumValue = (int)value;
            output.writeByte(flag);
            putVarInt32(output, enumValue);
        }

        //public override void setValue(Context ctx, object value)
        //{
        //    SerializableByteBuffer output = ctx.getBuffer();
        //    BaseEnum baseEnum = value as BaseEnum;
        //    EnumDef enumDef = ctx.getEnumDef(baseEnum.typeStr);

        //    if (enumDef == null)
        //    {
        //        throw new Exception("无法识别的枚举类型:" + baseEnum);
        //    }
        //    else
        //    {
        //        byte flag = Types.ENUM;
        //        output.writeByte(flag);
        //        int code = enumDef.Code;
        //        putVarInt32(output, code);

        //        //处理枚举索引
        //        int ordinal = 0;
        //        for (int i = 0; i < enumDef.Names.Length; i++)
        //        {
        //            string enumStr = enumDef.Names[i];
        //            if (enumStr == baseEnum.enumString)
        //            {
        //                ordinal = i;
        //                break;
        //            }
        //        }
        //        putVarInt32(output, ordinal);
        //    }
        //}
    }
}

