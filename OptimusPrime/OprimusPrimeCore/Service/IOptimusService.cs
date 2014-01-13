using BookSleeve;

namespace Prime.Optimus
{
    public interface IOptimusService
    {
        string Name { get; }
        IOptimusIn[] OptimusIn { get; }
        IOptimusOut[] OptimusOut { get; }
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