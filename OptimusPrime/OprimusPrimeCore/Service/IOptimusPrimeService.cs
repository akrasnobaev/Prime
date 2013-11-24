using BookSleeve;
using OptimusPrime.Generics;

namespace OptimusPrime.OprimusPrimeCore
{
    public interface IOptimusPrimeService : IGenericService
    {
        string Name { get; }
        IOptimusPrimeIn[] OptimusPrimeIn { get; }
        IOptimusPrimeOut[] OptimusPrimeOut { get; }
        RedisConnection Connection { get; }
        int DbPage { get; }
    }
}