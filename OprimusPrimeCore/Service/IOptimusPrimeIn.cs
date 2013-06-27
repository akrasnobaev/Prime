namespace OptimusPrime.OprimusPrimeCore.Service
{
    public interface IOptimusPrimeIn
    {
        string Name { get; }
        int ReadCounter { get; }
        IOptimusPrimeService Service { get; }

        bool TryGet<T>(out T result);
        T Get<T>();
        T[] GetRange<T>();

        void ChangeName(string newName);
    }
}