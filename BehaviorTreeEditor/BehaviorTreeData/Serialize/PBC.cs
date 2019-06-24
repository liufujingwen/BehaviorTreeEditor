using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BehaviorTreeData
{
    public static class SerializeHelper
    {
        public const char MESSAGE_END_FLAG = '|';
        public static Encoding UTF8 = new System.Text.UTF8Encoding(false);

        public static byte[] Serializable<T>(T instance) where T : Binary
        {
            Writer writer = new Writer();
            byte[] buffer = Serializable(writer, instance);
            writer.Close();
            return buffer;
        }

        public static byte[] Serializable<T>(Writer writer, T instance) where T : Binary
        {
            byte[] buffer = null;
            instance.Write(ref writer);
            writer.writer.Write(SerializeHelper.MESSAGE_END_FLAG);
            buffer = writer.GetBuffer();
            return buffer;
        }

        public static byte[] SerializeToFile<T>(T instance, string path) where T : Binary
        {
            Writer writer = new Writer();
            byte[] buffer = Serializable(writer, instance);
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

        public static T DeSerializable<T>(byte[] buffer) where T : Binary
        {
            Reader reader = new Reader();
            reader.Load(buffer, 0, buffer.Length);
            T instance = DeSerializable<T>(reader);
            reader.Close();
            return instance;
        }

        public static T DeSerializable<T>(Reader reader) where T : Binary
        {
            T instance = System.Activator.CreateInstance<T>();
            instance.Read(ref reader);
            reader.index++;
            reader.stream.Position++;
            return instance;
        }

        public static void DeSerializable(byte[] buffer, Binary binary)
        {
            Reader reader = new Reader();
            reader.Load(buffer, 0, buffer.Length);
            binary.Read(ref reader);
            reader.index++;
            reader.stream.Position++;
            reader.Close();
        }

        public static void DeSerializable(Reader reader, Binary binary)
        {
            binary.Read(ref reader);
            reader.index++;
            reader.stream.Position++;
        }

        public static T DeSerializableFromFile<T>(string path) where T : Binary
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
                    instance = DeSerializable<T>(reader);
                    reader.Close();
                }
            }
            return instance;
        }
    }
}
