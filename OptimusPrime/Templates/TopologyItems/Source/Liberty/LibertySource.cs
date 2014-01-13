using System.Collections.Generic;

namespace Prime.Liberty
{
    public class LibertySource<T> : ILibertySource<T>
    {
        public IPrimeFactory Factory { get; private set; }
        public string Name { get; private set; }
        public PrintableList<object> Collection { get; private set; }

        private readonly IList<LibertySourceReader<T>> _sourceReaders;

        public LibertySource(LibertyFactory factory, string name)
        {
            Factory = factory;
            Name = name;
            Collection = new PrintableList<object>();
            _sourceReaders = new List<LibertySourceReader<T>>();
        }

        public ISourceReader<T> CreateReader()
        {
            var sourceReader = new LibertySourceReader<T>(this);
            lock (_sourceReaders)
                _sourceReaders.Add(sourceReader);
            return sourceReader;
        }

        public void Release()
        {
            lock (_sourceReaders)
                foreach (var sourceReader in _sourceReaders)
                    sourceReader.AvailableData.Release();
        }
    }
}