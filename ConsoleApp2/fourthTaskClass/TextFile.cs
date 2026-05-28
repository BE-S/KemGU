using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace ConsoleApp2.fourthTaskClass
{
    [Serializable]
    class TextFile
    {
        public string FileName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public TextFile() { }

        public TextFile(string fileName, string content)
        {
            FileName = fileName;
            Content = content;
            CreatedAt = DateTime.Now;
            ModifiedAt = DateTime.Now;
        }

        public void SaveBinary(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, this);
            }
        }

        public static TextFile LoadBinary(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (TextFile)formatter.Deserialize(fs);
            }
        }

        public void SaveXml(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TextFile));
            using (StreamWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, this);
            }
        }

        public static TextFile LoadXml(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TextFile));
            using (StreamReader reader = new StreamReader(path))
            {
                return (TextFile)serializer.Deserialize(reader);
            }
        }
    }
}
