using BehaviorTreeEditor.Properties;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace BehaviorTreeEditor
{
    public static class XmlUtility
    {
        public static UTF8Encoding UTF8 = new System.Text.UTF8Encoding(false);

        public static bool Save<T>(string fileName, T data)
        {
            FileStream stream = File.Open(fileName, FileMode.Create, FileAccess.Write);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlTextWriter writer = new XmlTextWriter(stream, UTF8);
            writer.Formatting = Formatting.Indented;
            serializer.Serialize(writer, data);
            writer.Close();
            stream.Close();
            return true;
        }

        public static T Read<T>(string fileName)
        {
            FileStream stream = null;
            if (!File.Exists(fileName))
            {
                return default(T);
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                stream = File.OpenRead(fileName);
                XmlReader reader = XmlReader.Create(stream);
                T instance = (T)serializer.Deserialize(reader);
                stream.Close();
                return instance;
            }
            catch (InvalidOperationException ex)
            {
                if (stream != null)
                    stream.Close();
                return default(T);
            }
        }
    }
}
