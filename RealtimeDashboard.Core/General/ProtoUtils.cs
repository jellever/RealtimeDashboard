using System.IO;
using ProtoBuf;

namespace RealtimeDashboard.Core.General
{
    public static class ProtoUtils
    {
        public static byte[] Serialize<T>(T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Serializer.Serialize(stream, obj);
                stream.Flush();
                return stream.ToArray();
            }
        }

        public static void Serialize<T>(Stream stream, T obj)
        {
            Serializer.Serialize(stream, obj);
        }

        public static T Deserialize<T>(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                return Serializer.Deserialize<T>(stream);
            }
        }

        public static T Deserialize<T>(Stream stream)
        {
            return Serializer.Deserialize<T>(stream);
        }

        public static T CreateCopy<T>(T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Serializer.Serialize(stream, obj);
                stream.Flush();
                return Serializer.Deserialize<T>(stream);
            }
        }
    }
}
