using BookSleeve;

namespace OptimusPrime.OprimusPrimeCore
{
    public interface IOptimusPrimeService
    {
        string Name { get; }
        IOptimusPrimeIn[] OptimusPrimeIn { get; }
        IOptimusPrimeOut[] OptimusPrimeOut { get; }
        RedisConnection Connection { get; }
        int DbPage { get; }

        /// <summary>
        /// Инициализация сервиса.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Действия, которые выполняет сервис после инициализации до момента прекращения его работы.
        /// </summary>
        void DoWork();
    }
}