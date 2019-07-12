using Serializable.IO;

namespace Serializable.Proxy
{
	public class NullProxy : AbstractProxy
	{
		public NullProxy ()
		{
		}

		public override object getValue (Context ctx, byte flag) {
			// 0000 0001 (1 - 0x01)
			return null;
		}

		public override void setValue (Context ctx, object value) {
			SerializableByteBuffer output = ctx.getBuffer();
			byte flag = Types.NULL;
			output.writeByte((byte) flag);
		}
	}
}

