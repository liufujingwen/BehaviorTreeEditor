// #define DEBUG_INFO

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Serializable.Def;
using Serializable.Proxy;
using Serializable.IO;
//using ComponentAce.Compression.Libs.zlib;

namespace Serializable
{

	public class Context
	{
		public Context (Description desc, SerializableByteBuffer buffer)
		{
			this.desc = desc;
			this.buffer = buffer;

			this.stringTable = new RefTable<string> ();
			this.objectTable = new RefTable<object> ();
		}

		private Description desc;
		private RefTable<string> stringTable;
		private RefTable<object> objectTable;
		private SerializableByteBuffer buffer;

		public SerializableByteBuffer getBuffer ()
		{
			return buffer;
		}

		public EnumDef getEnumDef (int type)
		{
			EnumDef def = desc.getEnumDef (type);
			return def;
		}

        public EnumDef getEnumDef(string type)
        {
            EnumDef def = desc.getEnumDef(type);
            return def;
        }

        public TypeDef getTypeDef (int type)
		{
			TypeDef def = desc.getTypeDef (type);
			return def;
		}

		public TypeDef getTypeDef (String type)
		{
			TypeDef def = desc.getTypeDef (type);
			return def;
		}

		public object getValue (byte flag)
		{
			IProxy proxy = desc.getProxy (flag);
			return proxy.getValue (this, flag);
		}

		public void setValue (object value)
		{
			IProxy proxy = desc.getProxy (value);
			proxy.setValue (this, value);
		}

		public int getObjectRef (object value)
		{
			return objectTable.getKey (value);
		}

		public object getObjectRef (int index)
		{
			return objectTable.getValue (index);
		}

		public void putObjectRef (object value)
		{
			int code = objectTable.incrementAndGet ();
			objectTable.put (code, value);
		}

		public int getStringRef (String value)
		{
			return stringTable.getKey (value);
		}

		public string getStringRef (int index)
		{
			return (string)stringTable.getValue (index);
		}

		public void putStringRef (string value)
		{
			int code = stringTable.incrementAndGet ();
			stringTable.put (code, value);
		}
	}
}

