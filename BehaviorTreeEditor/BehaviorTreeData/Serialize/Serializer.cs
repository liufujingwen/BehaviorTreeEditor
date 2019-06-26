using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BehaviorTreeData
{
    public static class Serializer
    {
        public static Encoding UTF8 = new System.Text.UTF8Encoding(false);
        public static Dictionary<Type, int> TypeToValueDic = new Dictionary<Type, int>();
        public static Dictionary<int, Type> ValueToTypeDic = new Dictionary<int, Type>();

        static Serializer()
        {
            List<Type> list = new List<Type>();
            list.Add(typeof(AgentData));
            list.Add(typeof(NodeData));

            list.Add(typeof(IntField));
            list.Add(typeof(RepeatIntField));

            list.Add(typeof(FloatField));
            list.Add(typeof(RepeatFloatField));

            list.Add(typeof(LongField));
            list.Add(typeof(RepeatLongField));

            list.Add(typeof(DoubleField));
            list.Add(typeof(RepeatDoubleField));

            list.Add(typeof(EnumField));
            list.Add(typeof(ColorField));
            list.Add(typeof(BooleanField));


            TypeToValueDic.Clear();
            ValueToTypeDic.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                int typeValue = i;
                Type type = list[i];

                TypeToValueDic.Add(type, typeValue);
                ValueToTypeDic.Add(typeValue, type);
            }
        }

        public static int GetValueByType(Type type)
        {
            int value = -1;
            if (!TypeToValueDic.TryGetValue(type, out value))
                throw new Exception(string.Format("Type:{0} not register", type));
            return value;
        }

        public static Type GetTypeByValue(int value)
        {
            Type type = null;
            if (!ValueToTypeDic.TryGetValue(value, out type))
                throw new Exception(string.Format("Can not find type by value:{0} ", value));
            return type;
        }

        public static byte[] Serialize<T>(T instance) where T : Binary
        {
            Writer writer = new Writer();
            byte[] buffer = Serialize(writer, instance);
            writer.Close();
            return buffer;
        }

        public static byte[] Serialize<T>(Writer writer, T instance) where T : Binary
        {
            byte[] buffer = null;
            writer.Write(GetValueByType(instance.GetType()));
            instance.Write(ref writer);
            buffer = writer.GetBuffer();
            return buffer;
        }

        public static byte[] SerializeToFile<T>(T instance, string path) where T : Binary
        {
            Writer writer = new Writer();
            byte[] buffer = Serialize(writer, instance);
            writer.Close();
            if (buffer != null)
            {
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buffer, 0, buffer.Length);
                }
            }
            return buffer;
        }

        public static T DeSerialize<T>(byte[] buffer) where T : Binary
        {
            Reader reader = new Reader();
            reader.Load(buffer, 0, buffer.Length);
            T instance = DeSerialize<T>(reader);
            reader.Close();
            return instance;
        }

        public static T DeSerialize<T>(Reader reader) where T : Binary
        {
            int typeValue = reader.ReadInt32Variant();
            Type type = GetTypeByValue(typeValue);
            T instance = System.Activator.CreateInstance(type) as T;
            instance.Read(ref reader);
            return instance;
        }

        public static void DeSerialize(byte[] buffer, Binary binary)
        {
            Reader reader = new Reader();
            reader.Load(buffer, 0, buffer.Length);
            binary.Read(ref reader);
            reader.Close();
        }

        public static void DeSerialize(Reader reader, Binary binary)
        {
            binary.Read(ref reader);
        }

        public static T DeSerializeFromFile<T>(string path) where T : Binary
        {
            T instance = null;
            if (File.Exists(path))
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    Reader reader = new Reader();
                    reader.Load(buffer, 0, buffer.Length);
                    instance = DeSerialize<T>(reader);
                    reader.Close();
                }
            }
            return instance;
        }
    }
}
