using Serializable.IO;
using System.Text;

namespace Serializable.Def
{
	public class EnumDef //: IComparable<EnumDef>
	{
		public EnumDef ()
		{
		}

		/** 类型索引 */
		private int code;
		/**  类名 */
		private string type;
		/** 枚举名 */
		private string[] names;

		/** 类型索引 */
		public int Code { get { return code; } private set { code = value; } }
		/**  类名 */
		public string Type { get { return type; } private set { type = value; } }
		/** 枚举名 */
		public string[] Names { get { return names; } private set { names = value; } }

        public string[] Values { get { return names; } private set { names = value; } }

        public static EnumDef valueOf (int code, string type, string[] names)
		{
			EnumDef e = new EnumDef ();
			e.Code = code;
			e.Type = type;
			e.Names = names;
			return e;
		}

		public static EnumDef valueOf (SerializableByteBuffer buf)
		{
			// 类型, 类标识, (枚举类名长度, 枚举类名字节), 枚举数值数量, (名字长度, 名字字节)....
			short code = buf.readShort ();
			short nLen = buf.readShort ();
			byte[] nBytes = new byte[nLen];
			buf.readBytes (nBytes, 0, nLen);
			string type = Encoding.UTF8.GetString (nBytes);

			int len = buf.readShort ();
			string[] names = new string[len];
			for (int i = 0; i < len; i++) {
				int n = buf.readShort ();
				byte[] a = new byte[n];
				buf.readBytes (a, 0, n);
				names [i] = Encoding.UTF8.GetString (a);
			}

			return EnumDef.valueOf (code, type, names);
		}

		public void describe (SerializableByteBuffer buf)
		{
			// 类型, 类标识, (枚举类名长度, 枚举类名字节), 枚举数值数量, (名字长度, 名字字节)....
			byte[] bytes = Encoding.UTF8.GetBytes (Type);
			buf.writeByte ((byte)0x00);
			buf.writeShort ((short)Code);
			buf.writeShort ((short)bytes.Length);
			buf.writeBytes (bytes, 0, bytes.Length);

			buf.writeShort ((short)Names.Length);
			foreach (string name in Names) {
				byte[] nameBytes = Encoding.UTF8.GetBytes (name);
				buf.writeShort ((short)nameBytes.Length);
				buf.writeBytes (nameBytes, 0, nameBytes.Length);
			}
		}

		public string getValue (int ordinal)
		{
			return Names [ordinal];
		}


		// 实现接口方法
		//public int CompareTo (EnumDef obj) {
		//	return this.Code - obj.Code;
		//}
	}
}

