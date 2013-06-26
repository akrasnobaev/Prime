using System;
using OptimusPrime.Templates.FunctionalItems.FunctionalBlock;
using OptimusPrime.Templates.TopologyItems.Chain.Call;

namespace OptimusPrime.Factory.CallFactory
{
    public partial class CallFactory
    {
        public ICallChain<TIn, TOut> CreateChain<TIn, TOut>(IFunctionalBlock<TIn, TOut> functionalBlock)
        {
            return new CallChain<TIn, TOut>(functionalBlock.Action);
        }

        public ICallChain<TIn, TOut> CreateChain<TIn, TOut>(Func<TIn, TOut> func)
        {
            return new CallChain<TIn, TOut>(func);
        }

        public ICallChain<TIn, T3> AddFunctionalBlockToChain<TIn, TOut, T3>(ICallChain<TIn, TOut> chain,
                                                                            IFunctionalBlock<TOut, T3> functionalBlock)
        {
            return new CallChain<TIn, T3>(input => functionalBlock.Action(chain.Action(input)));
        }
    }
}