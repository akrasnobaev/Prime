using System.Collections.Generic;
using System.Threading;

namespace OptimusPrime.Templates
{
    public interface ICallSource <T> : ISource<T>
    {
        IList<object> Collection { get; }

        /// <summary>
        /// Автосгенерированное имя коллекции данных в лог-файле, которая получена
        /// в результате работы ICallSource
        /// </summary>
        string CollectionName { get; }

        /// <summary>
        /// Уведомляет все SourceReader о возможности чтения из источника.
        /// </summary>
        void Release();
    }
}