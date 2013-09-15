using System.Collections.Generic;
using OptimusPrime.Factory;

namespace OptimusPrime.Templates
{
    public class CallSource<T> : ICallSource<T>
    {
        public IFactory Factory { get; private set; }
        private readonly IList<SourceReader<T>> _sourceReaders;

        public CallSource(CallFactory factory)
        {
            Factory = factory;
            Collection = new List<T>();
            _sourceReaders = new List<SourceReader<T>>();
        }

        public ISourceReader<T> CreateReader()
        {
            var sourceReader = new SourceReader<T>(this);
            _sourceReaders.Add(sourceReader);
            return sourceReader;
        }

        public IList<T> Collection { get; private set; }
        public void Release()
        {
            foreach (var sourceReader in _sourceReaders)
                sourceReader.AvailableData.Release();
        }
    }
}