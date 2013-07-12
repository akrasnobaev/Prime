using System;
using OptimusPrime.Templates;

namespace OptimusPrime.Factory
{
    public partial class CallFactory
    {
        public ICallChain<TIn, TOut> CreateChain<TIn, TOut>(IFunctionalBlock<TIn, TOut> functionalBlock)
        {
            return new CallChain<TIn, TOut>(functionalBlock.Process);
        }

        public ICallChain<TIn, TOut> CreateChain<TIn, TOut>(Func<TIn, TOut> func)
        {
            return new CallChain<TIn, TOut>(func);
        }

        public ICallChain<TIn, T3> AddFunctionalBlockToChain<TIn, TOut, T3>(ICallChain<TIn, TOut> chain,
                                                                            IFunctionalBlock<TOut, T3> functionalBlock)
        {
            return new CallChain<TIn, T3>(input => functionalBlock.Process(chain.Action(input)));
        }
    }
}