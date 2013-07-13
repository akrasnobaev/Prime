using System.Collections;
using System.Collections.Generic;
using OptimusPrime.OprimusPrimeCore;

namespace OptimusPrime.Templates
{
    public class OptimusPrimeReader<T> : ISourceReader<T>
    {
        private OptimusPrimeIn input;

        public OptimusPrimeReader(string storageKey)
        {
            var inputService = new OptimusPrimeStabService();
            input = new OptimusPrimeIn(storageKey, inputService);
        }

        public IEnumerable GetCollectionNonTypized()
        {
            return GetCollection();
        }

        public T Get()
        {
            return input.Get<T>();
        }

        public bool TryGet(out T data)
        {
            return input.TryGet(out data);
        }

        public IEnumerable<T> GetCollection()
        {
            return input.GetRange<T>();
        }
    }
}