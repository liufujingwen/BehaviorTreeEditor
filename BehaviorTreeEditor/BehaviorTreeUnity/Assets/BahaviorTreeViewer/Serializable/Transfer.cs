// #define DEBUG_INFO

using System;
using System.IO;
using System.Collections.Generic;
using Serializable.Def;
using Serializable.Proxy;
using Serializable.IO;

namespace Serializable
{
	public class Transfer
	{
		public Transfer ()
		{
			this.desc = new Description ();
			this.defaultBuffer = new SerializableByteBuffer (4096);
		}

		// 枚举类型
		private Description desc;

		private SerializableByteBuffer defaultBuffer;

        public Description getDesc()
        {
            return desc;
        }

        private Context getContext(byte[] bytes) {
			SerializableByteBuffer buf = SerializableByteBuffer.Wrap (bytes);
			return new Context (desc, buf);
		}

		private Context getContext() {
			defaultBuffer.Clear ();
			return new Context (desc, defaultBuffer);
		}

		public object Decode (byte[] datas)
		{	
			Context ctx = getContext (datas);
			byte flag = ctx.getBuffer ().readByte ();
			return ctx.getValue (flag);
		}

		public byte[] Encode (object obj)
		{
			Context ctx = getContext();
			ctx.setValue(obj);
			SerializableByteBuffer buf = ctx.getBuffer();
			buf.Flip ();
			byte[] bytes = new byte[buf.Remaining];
			buf.readBytes (bytes, 0, buf.Remaining);
			return bytes;
		}
	}
}

