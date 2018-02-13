using System;
using System.IO;
using System.Xml.Serialization;

namespace elbemu_utils.Configuration
{
    // Inspired by ConfigFile.cs, https://github.com/xdanieldzd/MasterFudgeMk2
    [Serializable]
    public abstract class XmlConfiguration<T> where T : XmlConfiguration<T>, new()
    {
        protected static string DefaultName { get; } = typeof(T).Name + ".xml";

        public void Save()
        {
            Save(DefaultName);
        }

        public void Save(string filename)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Create))
            {
                new XmlSerializer(typeof(T)).Serialize(stream, this);
            }
        }

        public static T Load()
        {
            return Load(DefaultName);
        }

        public static T Load(string filename)
        {
            T configuration = new T();

            if (File.Exists(filename))
            {
                using (FileStream stream = new FileStream(filename, FileMode.Open))
                {
                    configuration = (T)new XmlSerializer(typeof(T)).Deserialize(stream);
                }
            }

            return configuration;
        }
    }
}
