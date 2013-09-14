using System;
using OptimusPrime.Templates;

namespace OptimusPrime.Factory
{
    public partial class CallFactory
    {
        public IChain<TIn, TOut> CreateChain<TIn, TOut>(IFunctionalBlock<TIn, TOut> functionalBlock)
        {
            return new CallChain<TIn, TOut>(this, functionalBlock.Process);
        }

        public IChain<TIn, TOut> CreateChain<TIn, TOut>(Func<TIn, TOut> func)
        {
            return new CallChain<TIn, TOut>(this, func);
        }

        public IChain<TIn, TOut> AddFunctionalBlockToChain<TIn, TOut, TMiddle>(ICallChain<TIn, TMiddle> chain,
                                                                            IFunctionalBlock<TMiddle, TOut> functionalBlock)
        {
            return new CallChain<TIn, TOut>(this, input => functionalBlock.Process(chain.Action(input)));
        }

        public IChain<TIn, TOut> LinkChainToChain<TIn, TOut, TMiddle>(IChain<TIn, TMiddle> first, IChain<TMiddle, TOut> second)
        {
            return AddFunctionalBlockToChain<TIn, TOut, TMiddle>(first as ICallChain<TIn,TMiddle>, second.ToFunctionalBlock());
        }
      
    }
}