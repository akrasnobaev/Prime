﻿namespace Prime
{
    public static partial class IFactoryExtensions
    {
        public static IChain<TIn, TOut> CreateChain<TIn, TOut>(this IPrimeFactory factory,
            IFunctionalBlock<TIn, TOut> function, string pseudoName = null)
        {
            return factory.CreateChain<TIn, TOut>(function.Process, pseudoName);
        }

        public static IReader<TOut> CreateReader<TOut>(this IPrimeFactory factory, ISource<TOut> source)
        {
            return source.Factory.CreateReciever(source).GetReader();
        }
    }
}