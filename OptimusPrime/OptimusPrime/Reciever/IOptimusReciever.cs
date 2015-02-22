namespace Prime.Optimus
{
    public interface IOptimusReciever<T> : IReciever<T>
    {
        IOptimusIn Input { get; }

        T Get();

        bool TryGet(out T data);

        T[] GetCollection();
    }
}
