using System;

namespace Prime
{
    public class SafeFlaggedValue<TOut>
    {
        public bool NotSafe;
        public TOut SubstituteValue;
    }

    public static partial class FactoryExtensions
    {
        public static IChain<TIn, TOut> SafetyFork<TIn, TOut>(
            this IChain<TIn, TOut> chain,
            Func<TIn, SafeFlaggedValue<TOut>> predicate,
            string pseudoName = null)
        {
            var block = chain.ToFunctionalBlock();
            return chain.Factory.CreateChain(
                new Func<TIn, TOut>(
                    z =>
                    {
                        var test = predicate(z);
                        if (test.NotSafe) return test.SubstituteValue;
                        else return block.Process(z);
                    }), pseudoName);
        }
    }
}