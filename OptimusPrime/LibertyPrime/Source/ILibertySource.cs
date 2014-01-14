namespace Prime.Liberty
{
    public interface ILibertySource<T> : ISource<T>
    {
        /// <summary>
        /// Коллекция данных, полученных из ISource.
        /// </summary>
        PrintableList<object> Collection { get; }

        /// <summary>
        /// Уведомляет все LibertySourceReader о возможности чтения из источника.
        /// </summary>
        void Release();
    }
}