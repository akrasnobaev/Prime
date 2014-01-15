namespace Prime
{
    public static partial class ChainExtension
    {
        public static Isolator<TIn, TOut> Isolate<TIn, TOut>(this IChain<TIn, TOut> chain)
        {
            return new Isolator<TIn, TOut>(chain);
        }
    }
}