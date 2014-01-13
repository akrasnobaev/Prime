namespace Prime
{
    public interface ISourceReader<T>
    {
        T Get();
        bool TryGet(out T data);
        T[] GetCollection();
    }
}