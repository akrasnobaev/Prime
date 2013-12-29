using System.Linq;
using System.Threading;
using BookSleeve;
using OptimusPrime.OprimusPrimeCore.Extension;

namespace OptimusPrime.OprimusPrimeCore
{
    public class OptimusPrimeIn : IOptimusPrimeIn
    {
        public string Name { get; private set; }
        public int ReadCounter { get; private set; }
        public IOptimusPrimeService Service { get; private set; }

        private readonly RedisSubscriberConnection subscriberChannel;
        private Semaphore semaphore;

        public OptimusPrimeIn(string storageKey, IOptimusPrimeService service)
        {
            Name = storageKey;
            Service = service;
            ReadCounter = 0;
            semaphore = initSemaphore(storageKey);

            subscriberChannel = Service.Connection.GetOpenSubscriberChannel();
            var subscribe = subscriberChannel.Subscribe(Name, OnMessageReceived);
            subscriberChannel.Wait(subscribe);
        }

        public void ChangeName(string newName, int readCounter = 0)
        {
            var unsubscribe = subscriberChannel.Unsubscribe(Name);
            subscriberChannel.Wait(unsubscribe);

            ReadCounter = readCounter;
            semaphore = initSemaphore(newName);

            var subscribe = subscriberChannel.Subscribe(newName, OnMessageReceived);
            subscriberChannel.Wait(subscribe);

            Name = newName;
        }

        /// <summary>
        /// Устанавливает синхронизатору начальное количество данных, которые имеются в прослушиваемом списке.
        /// </summary>
        /// <param name="listName">Имя списка данных</param>
        /// <returns>Синхронизатор</returns>
        private Semaphore initSemaphore(string listName)
        {
            var lengthTask = Service.Connection.Lists.GetLength(Service.DbPage, listName);
            var initialCount = (int)Service.Connection.Wait(lengthTask);
            return new Semaphore(initialCount, int.MaxValue);
        }

        ~OptimusPrimeIn()
        {
            subscriberChannel.Dispose();
            semaphore.Dispose();
        }

        private void OnMessageReceived(string s, byte[] bytes)
        {
            semaphore.Release();
        }

        public bool TryGet<T>(out T result)
        {
            if (TryGetBytes(out result))
            {
                ReadCounter++;
                semaphore.WaitOne();
                return true;
            }
            return false;
        }

        public T Get<T>()
        {
            semaphore.WaitOne();
            T result;
            if (TryGetBytes(out result))
            {
                ReadCounter++;
                return result;
            }
            throw new OptimusPrimeException(
                string.Format("После оповещения о записи, данные по ключу {0} не найдены", Name));
        }

        public T[] GetRange<T>()
        {
            var range = Service.Connection.Lists.Range(Service.DbPage, Name, ReadCounter, -1);
            var bytes = Service.Connection.Wait(range);

            var result = bytes.Select(SerializeExtension.Deserialize<T>).ToArray();
            ReadCounter += result.Length;

            for (var i = 0; i < result.Length; i++)
                semaphore.WaitOne();
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