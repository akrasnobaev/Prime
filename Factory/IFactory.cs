using OptimusPrime.Templates;
using System;
namespace OptimusPrime.Factory
{
    public interface IFactory
    {
        void Start();
        void Stop();
        string DumpDb();
        IChain<TIn, TOut> CreateChain<TIn, TOut>(Func<TIn, TOut> function);
        IChain<TIn, TOut> LinkChainToChain<TIn, TOut, TMiddle>(IChain<TIn, TMiddle> first, IChain<TMiddle, TOut> second);
        ISource<TData> CreateSource<TData>(ISourceBlock<TData> block);
        ISource<TOut> LinkSourceToChain<TIn, TOut>(ISource<TIn> source, IChain<TIn, TOut> chain);
  
    }
     
    public static partial class IFactoryExtensions
    {
        public static IChain<TIn, TOut> CreateChain<TIn, TOut>(this IFactory factory, IFunctionalBlock<TIn, TOut> function)
        {
           return factory.CreateChain<TIn, TOut>(function.Process);
        }

        public static ISourceReader<TOut> CreateReader<TOut>(this IFactory factory, ISource<TOut> source)
        {
            return source.CreateReader();
        }


    }
}