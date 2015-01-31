namespace Prime
{
    //todo CreateReciever method
    public interface ISource<TPublic>
    {
        /// <summary>
        ///     Ссылка на фабрику, от которой поражден ISource.
        /// </summary>
        IPrimeFactory Factory { get; }

        /// <summary>
        ///     Создает ISourceReader для чтения данных из ISource.
        /// </summary>
        /// <returns>ISourceReader для чтения данных из ISource</returns>
        IReciever<TPublic> CreateReciever(string readLogName = null);

        /// <summary>
        ///     Ключ коллекции данных, сгенерированной ISource.
        /// </summary>
        string Name { get; }
    }
}