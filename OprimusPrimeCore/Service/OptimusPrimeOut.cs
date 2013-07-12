using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace OptimusPrime.OprimusPrimeCore
{
    public class OptimusPrimeOut : IOptimusPrimeOut
    {
        public string Name { get; private set; }
        public IOptimusPrimeService Service { get; private set; }

        public OptimusPrimeOut(string storageKey, IOptimusPrimeService service)
        {
            Name = storageKey;
            Service = service;
        }

        public void Set(object value)
        {
            var bytes = Serialize(value);

            var taskAdd = Service.Connection.Lists.AddLast(Service.DbPage, Name, bytes);
            Service.Connection.Wait(taskAdd);

            var taskPublish = Service.Connection.Publish(Name, "");
            Service.Connection.Wait(taskPublish);
        }

        private static byte[] Serialize(object value)
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