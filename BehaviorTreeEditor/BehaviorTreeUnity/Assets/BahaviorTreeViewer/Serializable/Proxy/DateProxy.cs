using Serializable.IO;
using System;

namespace Serializable.Proxy
{
    public class DateProxy : AbstractProxy
    {
        public DateProxy()
        {
        }

        //private static DateTime START = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
        private static DateTime START = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

        public override object getValue(Context ctx, byte flag)
        {
            SerializableByteBuffer input = ctx.getBuffer();
            byte type = getFlagTypes(flag);
            if (type != Types.DATE_TIME)
            {
                throw new Exception("类型[" + Types.DATE_TIME + "], 不匹配[" + type + "]无效"); //WrongTypeException(Types.DATE_TIME, type);
            }

            // byte signal = getFlagSignal(flag);
            // if (signal == 0x00) {
            // #### 0000
            byte tag = input.readByte();
            long timestame = readVarInt64(input, tag);

            DateTime result = new DateTime(START.Ticks).AddSeconds(timestame).AddHours(SerializableHelper.zone);
            return result;
            // }
            // throw new WrongTypeException();
        }

        public override void setValue(Context ctx, object value)
        {
            SerializableByteBuffer output = ctx.getBuffer();
            byte flag = Types.DATE_TIME;
            // #### 0000
            output.writeByte(flag);
            // 1451577600
            long timestame = Convert.ToInt64((((DateTime)value) - START).TotalSeconds);
            if (timestame < 0)
            {
                // 时间为1970-1-1 0:00:00.000 时, 根据时区信息， 有机会 < 0, 导致出错, 所以要置 0
                timestame = 0;
            }
            putVarInt64(output, timestame);
        }
    }
}

