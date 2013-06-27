using System;

namespace OptimusPrime.Templates.FunctionalItems.FunctionalBlock
{
    public interface IFunctionalBlock<TIn, TOut>
    {
        Func<TIn, TOut> Action { get; }
    }
}