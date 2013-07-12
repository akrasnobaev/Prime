using System;

namespace OptimusPrime.Templates
{
    public interface IFunctionalBlock<TIn, TOut>
    {
        Func<TIn, TOut> Action { get; }
    }
}