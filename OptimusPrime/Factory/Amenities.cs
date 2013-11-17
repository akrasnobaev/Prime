using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimusPrime.Templates;

namespace OptimusPrime.Factory
{
    public static class IOptimusPrimeAmenities
    {
        public static IChain<TExternalInput, TExternalOutput>
            Link<TExternalInput, TExternalOutput, TMiddle>
            (this IChain<TExternalInput, TMiddle> firstChain, IChain<TMiddle, TExternalOutput> secondChain)
        {
            return firstChain.Factory.LinkChainToChain(firstChain, secondChain);
        }

        public static IChain<TExternalInput, TExternalOutput>
          Link<TExternalInput, TExternalOutput, TMiddle>
          (this IChain<TExternalInput, TMiddle> firstChain, IFunctionalBlock<TMiddle, TExternalOutput> block, string pseudoName = null)
        {
            return firstChain.Link(firstChain.Factory.CreateChain(new Func<TMiddle, TExternalOutput>(block.Process), pseudoName));
        }

        public static ISource<TSecondOutput>
            Link<TFirstOutput, TSecondOutput>
            (this ISource<TFirstOutput> source, IChain<TFirstOutput, TSecondOutput> chain)
        {
            return source.Factory.LinkSourceToChain(source, chain);
        }

        public static ISource<TSecondOutput>
            Link<TFirstOutput, TSecondOutput>
            (this ISource<TFirstOutput> source, IFunctionalBlock<TFirstOutput, TSecondOutput> chain, string pseudoName=null)
        {
            return source.Link(source.Factory.CreateChain(new Func<TFirstOutput,TSecondOutput>(chain.Process), pseudoName));
        }

        public static ISource<T>
            Where<T>
            (this ISource<T> source, Func<T, bool> predicate, string pseudoname = null)
        {
            return source.Factory.LinkSourceToFilter(source, source.Factory.CreateChain(predicate, pseudoname));
        }

        public static ISource<T>
            Where<T>
            (this ISource<T> source, IFunctionalBlock<T, bool> predicate, string pseudoName = null)
        {
            return source.Where(predicate.Process, pseudoName);
        }

        public static Fork<TIn, TOut> Fork<TIn, TOut>(this IChain<TIn, TOut> chain)
        {
            var forkBlock = new ForkBlock<TIn, TOut>(new Func<TIn, TOut>(chain.ToFunctionalBlock().Process));
            return new Fork<TIn, TOut>(
                chain.Factory.CreateChain(forkBlock.ForkedAction),
                chain.Factory.CreateSource(forkBlock.Source));
        }
    } 
}