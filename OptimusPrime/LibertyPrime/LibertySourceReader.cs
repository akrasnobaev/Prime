using System.Threading;

namespace Prime.Liberty
{
    public class LibertySourceReader<T> : ISourceReader<T>
    {
        private readonly ILibertySource<T> _libertySource;
        private int _readCount;

        public LibertySourceReader(ILibertySource<T> libertySource)
        {
            _libertySource = libertySource;
            _readCount = 0;
            AvailableData = new Semaphore(libertySource.Collection.Count, int.MaxValue);
        }

        public T Get()
        {
            AvailableData.WaitOne();
            if (_libertySource.Collection.Count > _readCount)
                return (T) _libertySource.Collection[_readCount++];
            throw new PrimeException("При попытке чтения из LibertySource данные не найдены");
        }

        public bool TryGet(out T data)
        {
            if (_libertySource.Collection.Count > _readCount)
            {
                AvailableData.WaitOne();
                data = (T) _libertySource.Collection[_readCount++];
                return true;
            }

            data = default(T);
            return false;
        }

        public T[] GetCollection()
        {
            var resultLength = _libertySource.Collection.Count - _readCount;
            var result = new T[resultLength];

            for (var i = 0; i < resultLength; i++)
            {
                result[i] = (T) _libertySource.Collection[_readCount++];
                AvailableData.WaitOne();
            }

            return result;
        }


        public Semaphore AvailableData { get; private set; }
    }
}