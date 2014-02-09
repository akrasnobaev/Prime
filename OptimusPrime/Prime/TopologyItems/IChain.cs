namespace Prime
{
    public interface IChain<TIn, TOut>
    {
        /// <summary>
        /// Приведение цепочки преобразования данных из типа TIn в тип TOut к функциональному блоку.
        /// </summary>
        /// <returns>Функциональный блок</returns>
        IFunctionalBlock<TIn, TOut> ToFunctionalBlock();

        /// <summary>
        /// Фабрика, которой пораждена данныя цепочка.
        /// </summary>
        IPrimeFactory Factory { get; }

        /// <summary>
        /// Имя коллекции данных типа TIn поступающих на вход в цепочку.
        /// </summary>
        string InputName { get; }

        /// <summary>
        /// Имя коллекции данных типа TOut получаемой на выходе из цепочки.
        /// </summary>
        string OutputName { get; }

        /// <summary>
        /// Помечает цепочку как использованную. Используется только внутри фабрики.
        /// </summary>
        void MarkUsed();
    }
}