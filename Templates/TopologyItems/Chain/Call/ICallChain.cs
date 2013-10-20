using System;

namespace OptimusPrime.Templates
{
    public interface ICallChain<TIn, TOut> : IChain<TIn, TOut>
    {
        Func<TIn, TOut> Action { get; }
        void SetInputName(string inputName);
    }
}