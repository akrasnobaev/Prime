using System.Collections.Generic;

namespace OptimusPrime.Templates
{
    public interface ISourceReader<T>
    {
        T Get();
        bool TryGet(out T data);
        IEnumerable<T> GetCollection();
    }
}