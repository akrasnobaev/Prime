using System.Collections.Generic;
using OptimusPrime.Factory;

namespace OptimusPrime.Templates
{
    public class CallSource<T> : ICallSource<T>
    {
        public IFactory Factory { get; private set; }
        private readonly IList<SourceReader<T>> _sourceReaders;

        public CallSource(CallFactory factory, string name)
        {
            Factory = factory;
            Name = name;
            Collection = new List<object>();
            _sourceReaders = new List<SourceReader<T>>();
        }

        public ISourceReader<T> CreateReader()
        {
            var sourceReader = new SourceReader<T>(this);
            lock (_sourceReaders)
                _sourceReaders.Add(sourceReader);
            return sourceReader;
        }

        public string Name { get; private set; }

        public IList<object> Collection { get; private set; }

        public void Release()
        {
            lock (_sourceReaders)
                foreach (var sourceReader in _sourceReaders)
                    sourceReader.AvailableData.Release();
        }
    }
}