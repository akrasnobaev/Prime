using System;
using System.Collections.Generic;
using Prime.Liberty;

namespace Prime
{
    public partial class LibertyFactory
    {
        public override IChain<TIn, TOut> CreateChain<TIn, TOut>(Func<TIn, TOut> func, string pseudoName = null)
        {
            var outputName = ServiceNameHelper.GetCollectionName<TOut>();
            // Если указан псевдоним, добавляем его в коллекцию псевдонимов имен.
            if (!string.IsNullOrEmpty(pseudoName))
                pseudoNames.Add(pseudoName, outputName);

            var logCollection = new PrintableList<object>();
            collections.Add(outputName, logCollection);

            var timestampCollection = new List<TimeSpan>();
            timestamps.Add(outputName, timestampCollection);

            var smartClone = new SmartClone<TOut>();
            return new LibertyChain<TIn, TOut>(this, inputData =>
            {
                var result = smartClone.Clone(func(inputData));
                // логирование результата работы цепочки.
                logCollection.Add(result);
                // логирование времени получения данных.
                timestampCollection.Add(Stopwatch.Elapsed);
                return result;
            }, outputName);
        }

        public IChain<TIn, TOut> CreateChain<TIn, TOut>(IFunctionalBlock<TIn, TOut> functionalBlock)
        {
            return CreateChain<TIn, TOut>(functionalBlock.Process);
        }
    }
}