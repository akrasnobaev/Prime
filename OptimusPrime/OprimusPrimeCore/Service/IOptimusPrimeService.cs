using BookSleeve;

namespace OptimusPrime.OprimusPrimeCore.Service
{
    public interface IOptimusPrimeService
    {
        string Name { get; }
        IOptimusPrimeIn[] OptimusPrimeIn { get; }
        IOptimusPrimeOut[] OptimusPrimeOut { get; }
        RedisConnection Connection { get; }
        int DbPage { get; }

        void Actuation();
    }
}