using System.Collections.Generic;

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
            T data;
            if (TryGet(out data))
                return data;

            callSource.AutoResetEvent.WaitOne();
            return callSource.Collection[readCount++];
        }

        public bool TryGet(out T data)
        {
            if (callSource.Collection.Count > readCount)
            {
                data = callSource.Collection[readCount++];
                return true;
            }

            data = default(T);
            return false;
        }

        public IEnumerable<T> GetCollection()
        {
            for (; readCount < callSource.Collection.Count; readCount++)
                yield return callSource.Collection[readCount];
        }

        public System.Collections.IEnumerable GetCollectionNonTypized()
        {
            return GetCollection();
        }
    }
}