using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prime
{
    public interface IReciever<T>
    {
        IPrimeFactory Factory { get; }

        /// <summary>
        ///     Создает IReader для чтения порций из IReciever.
        /// </summary>
        /// <returns>IDataReader для чтения порций из IReciever</returns>
        IReader<T> GetReader();

        /// <summary>
        ///     Ключ коллекции данных, сгенерированной IReciever.
        /// </summary>
        string InputName { get; }
    }
}
