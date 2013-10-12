using System.Collections.Generic;

namespace OptimusPrime.Templates
{
    public interface ICallSource <T> : ISource<T>
    {
        /// <summary>
        /// Коллекция данных, полученных из ISource.
        /// </summary>
        IList<object> Collection { get; }

        /// <summary>
        /// Уведомляет все SourceReader о возможности чтения из источника.
        /// </summary>
        void Release();
    }
}