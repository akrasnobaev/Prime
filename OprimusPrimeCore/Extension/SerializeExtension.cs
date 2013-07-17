using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace OptimusPrime.OprimusPrimeCore.Extension
{
    public static class SerializeExtension
    {
        public static T Deserialize<T>(this byte[] bytes)
        {
            var memoryStream = new MemoryStream();
            var binaryFormatter = new BinaryFormatter();
            memoryStream.Write(bytes, 0, bytes.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return (T)binaryFormatter.Deserialize(memoryStream);
        }
    }
}