using OptimusPrime.Templates;
using System;
namespace OptimusPrime.Factory
{
    public interface IFactory
    {
        void Start();
        void Stop();
        IChain<TIn, TOut> CreateChain<TIn, TOut>(Func<TIn, TOut> function);
        ISource<TData> CreateSource<TData>(ISourceBlock<TData> block);
        ISource<TOut> LinkSourceToChain<TIn, TOut>(ISource<TIn> source, IChain<TIn, TOut> chain);

    }

    public static partial class IFactoryExtensions
    {
        public static IChain<TIn, TOut> CreateChain<TIn, TOut>(this IFactory factory, IFunctionalBlock<TIn, TOut> function)
        {
           return factory.CreateChain<TIn, TOut>(function.Process);
        }
    }
}