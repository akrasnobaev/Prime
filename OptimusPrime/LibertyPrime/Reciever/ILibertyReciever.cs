namespace Prime.Liberty
{
    public interface ILibertyReciever<T> : IReciever<T>
    {
        T Get();

        bool TryGet(out T data);

        T[] GetCollection();
    }
}
