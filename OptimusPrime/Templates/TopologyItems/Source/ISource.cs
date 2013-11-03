using OptimusPrime.Factory;

namespace OptimusPrime.Templates
{
    public interface ISource<TPublic>
    {
        /// <summary>
        ///     Ссылка на фабрику, от которой поражден ISource.
        /// </summary>
        IFactory Factory { get; }

        /// <summary>
        ///     Создает ISourceReader для чтения данных из ISource.
        /// </summary>
        /// <returns>ISourceReader для чтения данных из ISource</returns>
        ISourceReader<TPublic> CreateReader();

        /// <summary>
        ///     Ключ коллекции данных, сгенерированной ISource.
        /// </summary>
        string Name { get; }
    }
}