using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


namespace Serializable.Def
{
	public class TypeDef //: IComparable<TypeDef>
	{
		private TypeDef ()
		{
		}

		// 类型索引
		private int code;
		// 类名
		private string typeStr;
        // 类Type
        private Type type;
        // 字段定义
        private List<FieldDef> fields;

		/** 类型索引 */
		public int Code { get { return code; } private set { code = value; } }

		/** 类名 */
		public string TypeStr { get { return typeStr; } private set { typeStr = value; } }

        /** 类Type */
        public Type Type { get { return type; } private set { type = value; } }

        /** 字段定义 */
        public List<FieldDef> Fields { get { return fields; } private set { fields = value; } }

		public static TypeDef valueOf (int code, string typeStr,Type type, List<FieldDef> fields)
		{
			TypeDef e = new TypeDef ();
			e.Code = code;
			e.Fields = fields;
			e.TypeStr = typeStr;
            e.Type = type;
			return e;
		}

		//public static TypeDef valueOf (SerializableByteBuffer buf)
		//{
		//	// 类型, 类标识, (类名长度, 类名字节), 属性数量, (名字长度, 名字字节)....
		//	short code = buf.readShort ();
		//	short nLen = buf.readShort ();
		//	byte[] nBytes = new byte[nLen];
		//	buf.readBytes (nBytes, 0, nLen);
		//	string clzName = Encoding.UTF8.GetString (nBytes);

		//	int len = buf.readShort ();
		//	List<FieldDef> fields = new List<FieldDef> (len);
		//	for (int i = 0; i < len; i++) {
		//		// 属性名
		//		int fcode = buf.readShort ();
		//		byte[] aField = new byte[fcode];
		//		buf.readBytes (aField, 0, fcode);
		//		string fname = Encoding.UTF8.GetString (aField);
		//		fields.Add (FieldDef.valueOf (i, fname));
		//	}

		//	return TypeDef.valueOf (code, clzName, fields);
		//}

		//public void describe (SerializableByteBuffer buf)
		//{
		//	// 类型, 类标识, (类名长度, 类名字节), 属性数量, (名字长度, 名字字节), (类型长度, 类型字节)....
		//	// 类型, 类标识, (类名长度, 类名字节), 属性数量, (名字长度, 名字字节)....
		//	byte[] defBytes = Encoding.UTF8.GetBytes (TypeStr);
		//	buf.writeByte ((byte)0x01);
		//	buf.writeShort ((short)Code);
		//	buf.writeShort ((short)defBytes.Length);
		//	buf.writeBytes (defBytes, 0, defBytes.Length);

		//	Fields.Sort ();

		//	buf.writeShort ((short)Fields.Count);
		//	foreach (FieldDef d in Fields) {
		//		byte[] fieldBytes = Encoding.UTF8.GetBytes (d.Name);
		//		buf.writeShort ((short)fieldBytes.Length);
		//		buf.writeBytes (fieldBytes, 0, fieldBytes.Length);
		//	}
		//}

		public PO newInstance ()
		{
			PO po = new PO (Type);
			return po;
		}

        public bool Contains(string key)
        {
            for (int i = 0; i < Fields.Count; i++)
            {
                if (Fields[i].Name == key)
                    return true;
            }
            return false;
        }

		// 实现接口方法
		//public int CompareTo (TypeDef obj)
		//{
		//	return this.Code - obj.Code;
		//}
	}
}

