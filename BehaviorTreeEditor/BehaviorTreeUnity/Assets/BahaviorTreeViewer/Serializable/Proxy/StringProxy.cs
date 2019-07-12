using Serializable.IO;
using System;
using System.IO;
using System.Text;

namespace Serializable.Proxy
{
    public class StringProxy : AbstractProxy
    {
        public StringProxy()
        {
        }

        private bool autoCompress = false;
        private int autoSize = 64;

        /**
		 * 是否对自动压缩字符串
	 	 */
        public bool AutoCompress
        {
            get { return autoCompress; }
            set { autoCompress = value; }
        }

        /**
		 * 自动压缩字符串长度
	 	 */
        public int AutoSize
        {
            get { return autoSize; }
            set { autoSize = value; }
        }


        public override object getValue(Context ctx, byte flag)
        {
            SerializableByteBuffer input = ctx.getBuffer();
            byte type = getFlagTypes(flag);
            if (type != Types.STRING)
            {
                throw new Exception("类型[" + Types.STRING + "], 不匹配[" + type + "]无效"); //WrongTypeException (Types.STRING, type);
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
                byte[] buf = new byte[len];
                input.readBytes(buf, 0, len);
                string result = Encoding.UTF8.GetString(buf);
                // 添加到字符串表
                ctx.putStringRef(result);
                return result;
            }
            else if (signal == 0x01)
            {
                // #### 0001
                byte tag = input.readByte();
                int refIndex = readVarInt32(input, tag);
                string result = ctx.getStringRef(refIndex);
                return result;
            }
            else if (signal == 0x02)
            {
                // #### 0010
                //byte tag = input.readByte();
                //int len = readVarInt32(input, tag);
                //if (input.Remaining < len)
                //{
                //    throw new EndOfStreamException();
                //}
                //byte[] buf = new byte[len];
                //input.readBytes(buf, 0, len);
                //// 压缩的字符串
                //byte[] unzip = QuickLZSharp.QuickLZ.decompress(buf);
                //String result = Encoding.UTF8.GetString(unzip);
                //// 添加到字符串表
                //ctx.putStringRef(result);
                //return result;
            }
            throw new Exception("类型[" + type + "], 无效的标记[" + signal + "]无效"); //UnknowSignalException (type, signal);
        }

        public override void setValue(Context ctx, object value)
        {
            SerializableByteBuffer output = ctx.getBuffer();
            byte flag = Types.STRING;
            string str = (string)value;
            int refIndex = ctx.getStringRef(str);
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
                ctx.putStringRef(str);

                byte[] bytes = Encoding.UTF8.GetBytes(str);
                //if (AutoCompress && bytes.Length > AutoSize)
                //{
                //    flag |= 0x02;
                //    bytes = QuickLZSharp.QuickLZ.compress(bytes, 1);
                //}
                output.writeByte(flag);

                int len = bytes.Length;
                putVarInt32(output, len);
                output.writeBytes(bytes, 0, len);
            }
        }
    }
}

