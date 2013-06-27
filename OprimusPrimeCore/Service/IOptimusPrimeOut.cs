namespace OptimusPrime.OprimusPrimeCore.Service
{
    public interface IOptimusPrimeOut
    {
        string Name { get; }
        IOptimusPrimeService Service { get; }

        void Set(object value);
    }
}