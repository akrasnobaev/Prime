namespace Prime.Optimus
{
    public interface IOptimusIn
    {
        string Name { get; }
        int ReadCounter { get; }
        IOptimusService Service { get; }

        bool TryGet<T>(out T result);
        T Get<T>();
        T[] GetRange<T>();

        void ChangeName(string newName, int readCounter = 0);
    }
}