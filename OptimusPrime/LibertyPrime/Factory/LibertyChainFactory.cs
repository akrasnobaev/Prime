using System;
using System.Collections.Generic;
using Prime.Liberty;

namespace Prime
{
    public partial class LibertyFactory
    {
        public IChain<TIn, TOut> CreateChain<TIn, TOut>(Func<TIn, TOut> func, string pseudoName = null)
        {
            var outputName = GetCollectionName<TOut>();
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

        public IChain<TIn, TOut> AddFunctionalBlockToChain<TIn, TOut, TMiddle>(ILibertyChain<TIn, TMiddle> chain,
            IFunctionalBlock<TMiddle, TOut> functionalBlock)
        {
            return CreateChain<TIn, TOut>(input => functionalBlock.Process(chain.Action(input)));
        }

        public IChain<TIn, TOut> LinkChainToChain<TIn, TOut, TMiddle>(IChain<TIn, TMiddle> first,
            IChain<TMiddle, TOut> second)
        {
            return AddFunctionalBlockToChain<TIn, TOut, TMiddle>(first as ILibertyChain<TIn, TMiddle>,
                second.ToFunctionalBlock());
        }
    }
}