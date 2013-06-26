using System.Collections.Generic;

namespace OptimusPrime.Templates.FunctionalItems.SourceReader
{
    public interface ISourceReader<T>
    {
        T Get();
        bool TryGet(out T data);
        IEnumerable<T> GetCollection();
    }
}