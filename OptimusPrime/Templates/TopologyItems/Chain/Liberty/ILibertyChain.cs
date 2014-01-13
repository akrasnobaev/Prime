using System;

namespace Prime.Liberty
{
    public interface ILibertyChain<TIn, TOut> : IChain<TIn, TOut>
    {
        Func<TIn, TOut> Action { get; }
        void SetInputName(string inputName);
    }
}