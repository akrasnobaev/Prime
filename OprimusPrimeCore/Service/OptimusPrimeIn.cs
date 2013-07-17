using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using BookSleeve;
using System.Linq;
using OptimusPrime.OprimusPrimeCore.Extension;

namespace OptimusPrime.OprimusPrimeCore
{
    public class OptimusPrimeIn : IOptimusPrimeIn
    {
        public string Name { get; private set; }
        public int ReadCounter { get; private set; }
        public IOptimusPrimeService Service { get; private set; }

        private readonly AutoResetEvent resetEvent;
        private readonly RedisSubscriberConnection subscriberChannel;

        public OptimusPrimeIn(string storageKey, IOptimusPrimeService service)
        {
            Name = storageKey;
            Service = service;
            ReadCounter = 0;

            resetEvent = new AutoResetEvent(false);
            subscriberChannel = Service.Connection.GetOpenSubscriberChannel();
            subscriberChannel.Subscribe(Name, OnMessageReceived);
        }

        public void ChangeName(string newName, int readCounter = 0)
        {
            ReadCounter = readCounter;
            subscriberChannel.Unsubscribe(Name);
            subscriberChannel.Subscribe(newName, OnMessageReceived);
            Name = newName;
        }

        ~OptimusPrimeIn()
        {
            resetEvent.Close();
        }

        private void OnMessageReceived(string s, byte[] bytes)
        {
            resetEvent.Set();
        }

        public bool TryGet<T>(out T result)
        {
            if (TryGetBytes(out result))
            {
                ReadCounter++;
                return true;
            }

            return false;
        }

        public T Get<T>()
        {
            T result;
            if (TryGetBytes(out result))
            {
                ReadCounter++;
                resetEvent.Reset();
                return result;
            }

            resetEvent.WaitOne();
            if (!TryGetBytes(out result))
                throw new OptimusPrimeException(
                    string.Format("После оповещения о записи, данные по ключу {0} не найдены", Name));
            ReadCounter++;
            return result;
        }

        public T[] GetRange<T>()
        {
            var range = Service.Connection.Lists.Range(Service.DbPage, Name, ReadCounter, -1);
            var bytes = Service.Connection.Wait(range);

            resetEvent.Reset();

            var result = bytes.Select(SerializeExtension.Deserialize<T>).ToArray();
            ReadCounter += result.Length;

            return result;
        }

        private bool TryGetBytes<T>(out T result)
        {
            var task = Service.Connection.Lists.Get(Service.DbPage, Name, ReadCounter);
            var bytes = Service.Connection.Wait(task);

            if (bytes == null)
            {
                result = default(T);
                return false;
            }

            result = bytes.Deserialize<T>();
            return true;
        }
    }
}