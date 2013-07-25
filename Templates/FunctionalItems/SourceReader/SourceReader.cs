using System.Collections;
using System.Collections.Generic;
using OptimusPrime.OprimusPrimeCore;

namespace OptimusPrime.Templates
{
    public class SourceReader<T> : ISourceReader<T>
    {
        private readonly ICallSource<T> callSource;
        private int readCount;

        public SourceReader(ICallSource<T> callSource)
        {
            this.callSource = callSource;
            readCount = 0;
        }

        public T Get()
        {
            callSource.Semaphore.WaitOne();
            if (callSource.Collection.Count > readCount)
                return callSource.Collection[readCount++];
            throw new OptimusPrimeException("При попытке чтения из CallSource данные не найдены");
        }

        public bool TryGet(out T data)
        {
            if (callSource.Collection.Count > readCount)
            {
                callSource.Semaphore.WaitOne();
                data = callSource.Collection[readCount++];
                return true;
            }

            data = default(T);
            return false;
        }

        public IEnumerable<T> GetCollection()
        {
            for (; readCount < callSource.Collection.Count; readCount++)
            {
                callSource.Semaphore.WaitOne();
                yield return callSource.Collection[readCount];
            }
        }

        public IEnumerable GetCollectionNonTypized()
        {
            return GetCollection();
        }
    }
}