using System.Threading;

namespace Prime.Liberty
{
    public class LibertyReciever<T> : ILibertyReciever<T>
    {
        protected readonly PrintableList<object> Collection;
        protected int ReadCount;

        public LibertyReciever(LibertyFactory factory, string inputName, PrintableList<object> collection)
        {
            Factory = factory;
            InputName = inputName;
            this.Collection = collection;
            ReadCount = 0;
            AvailableData = new Semaphore(collection.Count, int.MaxValue);
        }

        public virtual T Get()
        {
            AvailableData.WaitOne();
            if (Collection.Count > ReadCount)
            {
                var ret = get();
                return ret;
            }
            throw new PrimeException("При попытке чтения из LibertySource данные не найдены");
        }

        public virtual bool TryGet(out T data)
        {
            if (Collection.Count > ReadCount)
            {
                AvailableData.WaitOne();
                data = get();
                return true;
            }

            data = default(T);
            return false;
        }

        public virtual T[] GetCollection()
        {
            var resultLength = Collection.Count - ReadCount;
            var result = new T[resultLength];

            for (var i = 0; i < resultLength; i++)
            {
                result[i] = get();
                AvailableData.WaitOne();
            }
            return result;
        }

        public Semaphore AvailableData { get; private set; }

        private T get()
        {
            var ret = (T)(Collection[ReadCount++]);
            return ret;
        }

        public IPrimeFactory Factory { get; private set; }

        public IReader<T> GetReader()
        {
            return new LibertyReader<T>(this);
        }

        public string InputName { get; private set; }
    }
}
