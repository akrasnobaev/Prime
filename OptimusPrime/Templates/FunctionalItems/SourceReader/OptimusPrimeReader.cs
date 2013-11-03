using System.Collections;
using OptimusPrime.OprimusPrimeCore;

namespace OptimusPrime.Templates
{
    public class OptimusPrimeReader<T> : ISourceReader<T>
    {
        private readonly OptimusPrimeIn _input;

        public OptimusPrimeReader(string storageKey)
        {
            var inputService = new OptimusPrimeStabService();
            _input = new OptimusPrimeIn(storageKey, inputService);
        }

        public IEnumerable GetCollectionNonTypized()
        {
            return GetCollection();
        }

        public T Get()
        {
            return _input.Get<T>();
        }

        public bool TryGet(out T data)
        {
            return _input.TryGet(out data);
        }

        public T[] GetCollection()
        {
            return _input.GetRange<T>();
        }
    }
}