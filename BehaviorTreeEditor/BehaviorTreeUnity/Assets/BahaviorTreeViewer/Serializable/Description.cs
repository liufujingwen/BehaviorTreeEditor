// #define DEBUG_INFO

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Serializable.Def;
using Serializable.Proxy;
using System.Reflection;

namespace Serializable
{
    public class Description
    {
        public Description()
        {
            proxys[Types.NULL] = new NullProxy();
            proxys[Types.BOOLEAN] = new BooleanProxy();
            proxys[Types.BYTE_ARRAY] = new BytesProxy();
            proxys[Types.NUMBER] = new NumberProxy();
            proxys[Types.DATE_TIME] = new DateProxy();
            proxys[Types.ENUM] = new EnumProxy();
            proxys[Types.STRING] = new StringProxy();
            proxys[Types.ARRAY] = new ArrayProxy();
            proxys[Types.COLLECTION] = new CollectionProxy();
            proxys[Types.MAP] = new MapProxy();
            proxys[Types.OBJECT] = new ObjectProxy();
        }

        // 枚举类型
        public Dictionary<string, EnumDef> enumDefs = new Dictionary<string, EnumDef>();
        public Dictionary<int, EnumDef> enumIdxs = new Dictionary<int, EnumDef>();
        // 对象类型
        public Dictionary<string, TypeDef> typeDefs = new Dictionary<string, TypeDef>();
        public Dictionary<int, TypeDef> typeIdxs = new Dictionary<int, TypeDef>();
        // PROXY
        private Dictionary<byte, IProxy> proxys = new Dictionary<byte, IProxy>();

        public IProxy getProxy(byte flag)
        {
            byte type = (byte)(0xF0 & flag);
            if (type == 0)
            {
                type = flag;
            }
            IProxy proxy = proxys[type];
            if (proxy != null)
            {
                return proxy;
            }

            throw new Exception("类型[" + type + "]无效");// WrongTypeException (type);

        }

        public IProxy getProxy(object value)
        {
            byte type;
            // 类型
            if (value == null)
            {
                type = Types.NULL;
            }
            else if (value is int || value is uint
                     || value is long || value is ulong
                     || value is short || value is ushort
                     || value is byte || value is float || value is double)
            {
                type = Types.NUMBER;
            }
            else if (value is string)
            {
                type = Types.STRING;
            }
            else if (value is bool)
            {
                type = Types.BOOLEAN;
            }
            else if (value is DateTime)
            {
                type = Types.DATE_TIME;
            }
            else if (value is byte[])
            {
                type = Types.BYTE_ARRAY;
            }
            else if (value is Array)
            {
                type = Types.ARRAY;
            }
            else if (value is PO)
            {
                type = Types.OBJECT;
            }
            else if (value is BaseEnum)
            {
                type = Types.ENUM;
            }
            else if (value is Enum)
            {
                type = Types.ENUM;
            }
            else if (value.GetType().Name == "ILEnumTypeInstance")
            {
                type = Types.ENUM;
            }
            else if (value is IDictionary)
            {
                type = Types.MAP;
            }
            else if (value is ICollection)
            {
                type = Types.COLLECTION;
            }
            else
            {
                throw new Exception("类型[" + value.GetType() + "]无效"); //WrongTypeException(value.GetType());
            }
            if (proxys.ContainsKey(type))
            {
                IProxy proxy = proxys[type];
                if (proxy != null)
                {
                    return proxy;
                }
            }

            throw new Exception("类型[" + type + "]无效");// WrongTypeException (type);
        }

        public EnumDef getEnumDef(int type)
        {
            if (!enumIdxs.ContainsKey(type))
            {
                return null;
            }
            EnumDef def = enumIdxs[type];
            return def;
        }

        public EnumDef getEnumDef(String type)
        {
            if (!enumDefs.ContainsKey(type))
            {
                return null;
            }
            EnumDef def = enumDefs[type];
            return def;
        }

        public TypeDef getTypeDef(int type)
        {
            if (!typeIdxs.ContainsKey(type))
            {
                return null;
            }
            TypeDef def = typeIdxs[type];
            return def;
        }

        public TypeDef getTypeDef(String type)
        {
            if (!typeDefs.ContainsKey(type))
            {
                return null;
            }
            TypeDef def = typeDefs[type];
            return def;
        }

        //public void AddEnumType(Type type)
        //{
        //    string typeStr = type.FullName.Replace("+", ".");
        //    EnumDef def = getEnumDef(typeStr);
        //    if (def != null)
        //        throw new Exception("已存在枚举:" + typeStr);

        //    FieldInfo[] infos = type.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        //    string[] names = new string[infos.Length];
        //    for (int i = 0; i < infos.Length; i++)
        //    {
        //        FieldInfo info = infos[i];
        //        names[i] = info.Name;
        //    }
        //    enumDefs.Add(typeStr, EnumDef.valueOf(enumDefs.Count + 1, typeStr, names));
        //}

        public void AddClassType(Type type)
        {
            string typeStr = type.FullName.Replace("+", ".");
            TypeDef def = getTypeDef(typeStr);
            if (def != null)
                throw new Exception("已存在类:" + typeStr);

            FieldInfo[] infos = type.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            List<FieldDef> fields = new List<FieldDef>(infos.Length);
            for (int i = 0; i < infos.Length; i++)
            {
                FieldInfo info = infos[i];
                FieldDef tempDef = FieldDef.valueOf(i, info.Name);
                fields.Add(tempDef);
            }

            fields.Sort(delegate (FieldDef f1, FieldDef f2)
            {
                return f1.Name.CompareTo(f2.Name);
            });

            def = TypeDef.valueOf(typeDefs.Count + 1, typeStr, type, fields);
            typeIdxs[def.Code] = def;
            typeDefs.Add(typeStr, def);

        }
    }
}

