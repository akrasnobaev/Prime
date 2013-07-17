using System.Collections.Generic;

namespace OptimusPrime.OprimusPrimeCore.Logger
{
    public interface IOptimusPrimeLogger
    {
        T Get<T>(string storageKey);
        IEnumerable<T> GetRange<T>(string storageKey);
    }
}