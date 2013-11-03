using System.Collections;
using System.Threading;
using OptimusPrime.OprimusPrimeCore;

namespace OptimusPrime.Templates
{
    public class SourceReader<T> : ISourceReader<T>
    {
        private readonly ICallSource<T> _callSource;
        private int _readCount;

        public SourceReader(ICallSource<T> callSource)
        {
            _callSource = callSource;
            _readCount = 0;
            AvailableData = new Semaphore(callSource.Collection.Count, int.MaxValue);
        }

        public T Get()
        {
            AvailableData.WaitOne();
            if (_callSource.Collection.Count > _readCount)
                return (T) _callSource.Collection[_readCount++];
            throw new OptimusPrimeException("При попытке чтения из CallSource данные не найдены");
        }

        public bool TryGet(out T data)
        {
            if (_callSource.Collection.Count > _readCount)
            {
                AvailableData.WaitOne();
                data = (T) _callSource.Collection[_readCount++];
                return true;
            }

            data = default(T);
            return false;
        }

        public T[] GetCollection()
        {
            var resultLength = _callSource.Collection.Count - _readCount;
            var result = new T[resultLength];

            for (var i = 0; i < resultLength; i++)
            {
                result[i] = (T) _callSource.Collection[_readCount++];
                AvailableData.WaitOne();
            }

            return result;
        }

        public IEnumerable GetCollectionNonTypized()
        {
            return GetCollection();
        }

        public Semaphore AvailableData { get; private set; }
    }
}