using System;
using System.Diagnostics;

namespace Prime
{
    public interface IPrimeFactory
    {
        /// <summary>
        /// Starts factory.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops factory.
        /// </summary>
        void Stop();

        /// <summary>
        /// Dumps all data to file with automatically generated name.
        /// </summary>
        /// <returns>Generated file name.</returns>
        string DumpDb();

        /// <summary>
        /// Создание элементарной цепочки преобразования данных из типа TIn в тип TOut.
        /// </summary>
        /// <typeparam name="TIn">Исходный тип данных.</typeparam>
        /// <typeparam name="TOut">Результирующий тип данных.</typeparam>
        /// <param name="function">Функция преобразования данных из типа TIn в TOut</param>
        /// <param name="pseudoName">Псевдоним коллекции данных, полученных в ходе работы цепочки.</param>
        /// <returns>Цепочка преобразования данных.</returns>
        IChain<TIn, TOut> CreateChain<TIn, TOut>(Func<TIn, TOut> function, string pseudoName = null);

        /// <summary>
        /// Объединение двух цепочек в одну.
        /// Перваяцепочка преобразует данные из типа TIn в тип TMiddle,
        /// вторая преобразует данные из типа TMiddle в тип TOut.
        /// Итоговая цепочка преобразует данные из типа TIn в тип TOut.
        /// </summary>
        /// <typeparam name="TIn">Исходный тип данных.</typeparam>
        /// <typeparam name="TOut">Итоговый тип данных.</typeparam>
        /// <typeparam name="TMiddle">Промежуточый тип данных.</typeparam>
        /// <param name="first">Цепочка преобразования данных из типа TIn в тип TMiddle.</param>
        /// <param name="second">Цепочка преобразования данных из типа TMiddle в тип TOut.</param>
        /// <returns>Цепочка преобразования данных из типа TIn в тип TOut.</returns>
        IChain<TIn, TOut> LinkChainToChain<TIn, TOut, TMiddle>(IChain<TIn, TMiddle> first, IChain<TMiddle, TOut> second);

        /// <summary>
        /// Создание источника данных типа TData.
        /// </summary>
        /// <typeparam name="TData">Тип генерируемых данных.</typeparam>
        /// <param name="block">Источник данных типа TData.</param>
        /// <param name="pseudoName">Псевдоним коллекции данных, полученных из источника данных.</param>
        /// <returns>Источник данных типа TData.</returns>
        ISource<TData> CreateSource<TData>(IEventBlock<TData> block, string pseudoName = null);

        /// <summary>
        /// Объединение источника данных и цепочки в источник данных.
        /// Источник данных генерирует данные типа TIn.
        /// Цепочка преобразует данные типа TIn в тип TOut.
        /// Итоговый тип данных генерирует данне типа TOut.
        /// </summary>
        /// <typeparam name="TIn">Исходный тип данных.</typeparam>
        /// <typeparam name="TOut">Итоговый тип данных.</typeparam>
        /// <param name="source">Источник данных типа TIn.</param>
        /// <param name="chain">Цепочка преодразования данных из типа TIn в тип TOut.</param>
        /// /// <param name="pseudoName">Псевдоним. Используется при работе с лог-файлом.</param>
        /// <returns>Источник данных типа TOut.</returns>
        ISource<TOut> LinkSourceToChain<TIn, TOut>(ISource<TIn> source, IChain<TIn, TOut> chain,
            string pseudoName = null);

        /// <summary>
        /// Создание источника данных из источника данных и фильтрующего блока.
        /// Фильтрующий блок пропускает только чать данных типа T сгенерированных источником данных.
        /// </summary>
        /// <typeparam name="T">Тип данных.</typeparam>
        /// <param name="source">Источник данных типа T.</param>
        /// <param name="filterBlock">Фильтрующий блок.</param>
        /// <param name="pseudoName">Псевдоним. Используется при работе с лог-файлом.</param>
        /// <returns>Источник отфильтрованных данных типа T.</returns>
        ISource<T> LinkSourceToFilter<T>(ISource<T> source, IFunctionalBlock<T, bool> filterBlock,
            string pseudoName = null);


        void RegisterGenericService(IGenericService service);

        /// <summary>
        /// Вывод на консоль значений по указанному выходу.
        /// </summary>
        /// <param name="InputName">Имя выхода цепочки или источника.</param>
        /// <param name="ToString">Способ преобразования в строку</param>
        void ConsoleLog<T>(string InputName, PrintableList<T>.ToString ToString = null);

        /// <summary>
        /// Create reciever from Event.
        /// </summary>
        IReciever<TOut> CreateReciever<TOut>(ISource<TOut> source, string readLogName = null);

        Stopwatch Stopwatch { get; }
        bool IsLogging { get; }
    }
}