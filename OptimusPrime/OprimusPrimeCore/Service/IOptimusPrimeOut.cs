namespace OptimusPrime.OprimusPrimeCore
{
    public interface IOptimusPrimeOut
    {
        string Name { get; }
        IOptimusPrimeService Service { get; }

        void Set(object value);
    }
}