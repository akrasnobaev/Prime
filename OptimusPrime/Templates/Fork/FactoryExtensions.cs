using System;
using Prime.Templates;

namespace Prime
{
    public static partial class FactoryExtensions
    {
        public static Fork<TIn, TOut> Fork<TIn, TOut>(this IChain<TIn, TOut> chain)
        {
            var forkBlock = new ForkBlock<TIn, TOut>(new Func<TIn, TOut>(chain.ToFunctionalBlock().Process));
            return new Fork<TIn, TOut>(
                chain.Factory.CreateChain(forkBlock.ForkedAction),
                chain.Factory.CreateSource(forkBlock.Source));
        }
    }
}