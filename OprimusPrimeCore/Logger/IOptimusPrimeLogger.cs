using System.Collections.Generic;

namespace OptimusPrime.OprimusPrimeCore
{
    public interface IOptimusPrimeLogger
    {
        bool LoadDb(string filePath);
        T Get<T>(string storageKey);
        IEnumerable<T> GetRange<T>(string storageKey);
    }
}