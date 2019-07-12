using Serializable.IO;
using System;
using System.IO;

namespace Serializable.Proxy
{
	public class BytesProxy : AbstractProxy
	{
		public BytesProxy ()
		{
		}

		public override object getValue (Context ctx, byte flag)
		{
			SerializableByteBuffer input = ctx.getBuffer ();
			byte type = getFlagTypes (flag);
			if (type != Types.BYTE_ARRAY) {
				throw new Exception("类型[" + Types.BYTE_ARRAY + "], 不匹配[" + type + "]无效");  //WrongTypeException (Types.BYTE_ARRAY, type);
			}

			// byte signal = getFlagSignal(flag);
			// if (signal == 0x00) {
			// #### 0000
			byte tag = input.readByte ();
			int len = readVarInt32 (input, tag);
			if (input.Remaining < len) {
				throw new EndOfStreamException ();
			}
			byte[] result = new byte[len];
			input.readBytes (result, 0, len);
			return result;
			// }
			// throw new WrongTypeException();
		}

		public override void setValue (Context ctx, object value)
		{
			SerializableByteBuffer output = ctx.getBuffer ();
			byte flag = Types.BYTE_ARRAY;
			// #### 0000
			output.writeByte (flag);
			byte[] array = (byte[])value;
			int len = array.Length;
			putVarInt32 (output, len);
			output.writeBytes (array, 0, len);
		}
	}
}

