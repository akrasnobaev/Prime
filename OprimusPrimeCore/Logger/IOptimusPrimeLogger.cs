using System.Collections.Generic;

namespace OptimusPrime.OprimusPrimeCore
{
    public interface IOptimusPrimeLogger
    {
        bool LoadFile(string filePath);
        
        T Get<T>(string key) where T : class;
        IEnumerable<T> GetRange<T>(string key);
        bool TryGet<T> (string key, out T result) where T : class;
    }
}