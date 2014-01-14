using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Prime
{
    public static class SerializeExtension
    {
        public static T Deserialize<T>(this byte[] bytes)
        {
            var memoryStream = new MemoryStream();
            var binaryFormatter = new BinaryFormatter();
            memoryStream.Write(bytes, 0, bytes.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return (T) binaryFormatter.Deserialize(memoryStream);
        }

        public static byte[] Serialize(this object value)
        {
            if (value == null)
                return new byte[0];
            var binaryFormatter = new BinaryFormatter();
            var memoryStream = new MemoryStream();

            binaryFormatter.Serialize(memoryStream, value);
            return memoryStream.ToArray();
        }
    }
}