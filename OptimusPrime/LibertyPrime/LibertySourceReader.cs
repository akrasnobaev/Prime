using System.Threading;

namespace Prime.Liberty
{
    public class LibertySourceReader<T> : ISourceReader<T>
    {
        private readonly ILibertySource<T> libertySource;
        private int readCount;

        public LibertySourceReader(ILibertySource<T> libertySource)
        {
            this.libertySource = libertySource;
            readCount = 0;
            AvailableData = new Semaphore(libertySource.Collection.Count, int.MaxValue);
        }

        public T Get()
        {
            AvailableData.WaitOne();
            if (libertySource.Collection.Count > readCount)
                return get();
            throw new PrimeException("При попытке чтения из LibertySource данные не найдены");
        }

        public bool TryGet(out T data)
        {
            if (libertySource.Collection.Count > readCount)
            {
                AvailableData.WaitOne();
                data = get();
                return true;
            }

            data = default(T);
            return false;
        }

        public T[] GetCollection()
        {
            var resultLength = libertySource.Collection.Count - readCount;
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
            return (T) libertySource.Collection[readCount++];
        }
    }
}