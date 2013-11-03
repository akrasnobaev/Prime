using System.Collections;

namespace OptimusPrime.Templates
{
    public interface ISourceReader
    {
        IEnumerable GetCollectionNonTypized();
    }

    public interface ISourceReader<T> : ISourceReader
    {
        T Get();
        bool TryGet(out T data);
        T[] GetCollection();
    }
}