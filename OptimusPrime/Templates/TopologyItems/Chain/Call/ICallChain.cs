using System;

namespace OptimusPrime.Templates.TopologyItems.Chain.Call
{
    public interface ICallChain<TIn, TOut> : IChain<TIn, TOut>
    {
        Func<TIn, TOut> Action { get; }
    }
}