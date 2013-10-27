using System;
using System.Collections.Generic;
using OptimusPrime.Templates;

namespace OptimusPrime.Factory
{
    public partial class CallFactory
    {
        public IChain<TIn, TOut> CreateChain<TIn, TOut>(Func<TIn, TOut> func, string pseudoName = null)
        {
            var logCollection = new List<object>();
            var outputName = GetCollectionName<TOut>();

            // Если указан псевдоним, добавляем его в коллекцию псевдонимов имен.
            if (!string.IsNullOrEmpty(pseudoName))
                _pseudoNames.Add(pseudoName, outputName);

            _collections.Add(outputName, logCollection);

            return new CallChain<TIn, TOut>(this, inputData =>
                {
                    var result = func(inputData);
                    // логирование результата работы цепочки.
                    logCollection.Add(result);
                    return result;
                }, outputName);
        }

        public IChain<TIn, TOut> CreateChain<TIn, TOut>(IFunctionalBlock<TIn, TOut> functionalBlock)
        {
            return CreateChain<TIn, TOut>(functionalBlock.Process);
        }

        public IChain<TIn, TOut> AddFunctionalBlockToChain<TIn, TOut, TMiddle>(ICallChain<TIn, TMiddle> chain,
                                                                            IFunctionalBlock<TMiddle, TOut> functionalBlock)
        {
            return CreateChain<TIn, TOut>(input => functionalBlock.Process(chain.Action(input)));
        }

        public IChain<TIn, TOut> LinkChainToChain<TIn, TOut, TMiddle>(IChain<TIn, TMiddle> first, IChain<TMiddle, TOut> second)
        {
            return AddFunctionalBlockToChain<TIn, TOut, TMiddle>(first as ICallChain<TIn,TMiddle>, second.ToFunctionalBlock());
        }
      
    }
}