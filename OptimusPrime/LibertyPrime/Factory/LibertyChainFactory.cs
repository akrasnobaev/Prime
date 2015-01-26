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

            var smartClone = new SmartClone<TOut>();
            Func<TIn, TOut> action = inputData => smartClone.Clone(func(inputData));

            // Логируем данные и временные отпечатки только в том случае, если логирование включено.
            if (IsLogging)
            {
                var logCollection = new PrintableList<object>();
                var timestampCollection = new List<TimeSpan>();

                collections.Add(outputName, logCollection);
                timestamps.Add(outputName, timestampCollection);

                action = inputData =>
                {
                    var outputData = smartClone.Clone(func(inputData));
                    logCollection.Add(outputData);
                    timestampCollection.Add(Stopwatch.Elapsed);
                    return outputData;
                };
            }

            return new LibertyChain<TIn, TOut>(this, action, outputName);
        }

        public IChain<TIn, TOut> CreateChain<TIn, TOut>(IFunctionalBlock<TIn, TOut> functionalBlock)
        {
            return CreateChain<TIn, TOut>(functionalBlock.Process);
        }
    }
}