using OptimusPrime.Templates;
using System;
namespace OptimusPrime.Factory
{
    public interface IFactory
    {
        /// <summary>
        /// Запуск фабрики.
        /// </summary>
        void Start();

        /// <summary>
        /// Остановка фабрики.
        /// </summary>
        void Stop();
        /// <summary>
        /// Записывает накопившиеся в системе данные в файл с автоматически сгенерированным именем.
        /// </summary>
        /// <returns>Автоматически сгенерированное имя файла, в который записываются сгенерированные в системе данные.</returns>
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
        ISource<TData> CreateSource<TData>(ISourceBlock<TData> block, string pseudoName = null);

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
        /// <returns>Источник данных типа TOut.</returns>
        ISource<TOut> LinkSourceToChain<TIn, TOut>(ISource<TIn> source, IChain<TIn, TOut> chain);

        //TODO: написать документацию
        ISource<T> LinkSourceToFilter<T>(ISource<T> source, IFunctionalBlock<T, bool> filter, string pseudoName = null);

    }
     
    public static partial class IFactoryExtensions
    {
        public static IChain<TIn, TOut> CreateChain<TIn, TOut>(this IFactory factory, IFunctionalBlock<TIn, TOut> function)
        {
           return factory.CreateChain<TIn, TOut>(function.Process);
        }

        public static ISourceReader<TOut> CreateReader<TOut>(this IFactory factory, ISource<TOut> source)
        {
            return source.CreateReader();
        }


    }
}