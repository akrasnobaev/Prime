using System.Collections.Generic;
using OptimusPrime.OprimusPrimeCore.ConsoleLog;

namespace OptimusPrime.Templates
{
    public interface ICallSource <T> : ISource<T>
    {
        /// <summary>
        /// Коллекция данных, полученных из ISource.
        /// </summary>
        PrintableList<object> Collection { get; }

        /// <summary>
        /// Уведомляет все SourceReader о возможности чтения из источника.
        /// </summary>
        void Release();
    }
}