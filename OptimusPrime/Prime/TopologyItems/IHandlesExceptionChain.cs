using System;
using OptimusPrime;
using Prime.Liberty;

namespace Prime
{
    public interface IHandlesExceptionChain <TIn, TInnerOut> : ILibertyChain<TIn, PrimeData<TInnerOut>>
    {
        /// <summary>
        /// Конфигурирование цепочки методом, которым нужно обрабатыватьисключения.
        /// По-умолчанию исключения не обрабатываются.
        /// </summary>
        /// <param name="handler">Способ обработки исключения</param>
        void HandleExceptions(Func<Exception, TInnerOut> handler);

        /// <summary>
        /// Приведение цепочки преобразования данных из типа TIn в тип TInnerOut к функциональному блоку.
        /// </summary>
        /// <returns>Функциональный блок</returns>
        new IFunctionalBlock<TIn, TInnerOut> ToFunctionalBlock();
    }
}