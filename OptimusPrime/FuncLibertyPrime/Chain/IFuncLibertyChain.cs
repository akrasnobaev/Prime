using System;
using System.Linq.Expressions;
using Prime;

namespace OptimusPrime.FuncLibertyPrime
{
    public interface IFuncLibertyChain<TIn, TOut> : IChain<TIn, TOut>
    {
        Expression<Func<TIn, TOut>> Expression { get; }
    }
}