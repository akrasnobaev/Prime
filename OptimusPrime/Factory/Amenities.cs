using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimusPrime.Templates;
using System.IO;
using Eurobot.Services;

namespace OptimusPrime.Factory
{
    public static partial class FactoryExtensions
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
            return source.Link(chain.Process, pseudoName);
        }

        public static ISource<TSecondOutput>
            Link<TFirstOutput, TSecondOutput>
            (this ISource<TFirstOutput> source, Func<TFirstOutput, TSecondOutput> chain, string pseudoName = null)
        {
            return source.Link(source.Factory.CreateChain(new Func<TFirstOutput, TSecondOutput>(chain), pseudoName));
        }

        public static void DumpDb(this IFactory factory, string filename)
        {
            var f = factory.DumpDb();
            File.Move(f, filename);
        }

        public static SourceBlock<T> CreateCustomLogger<T>(this IFactory factory, string loggingName)
        {
            var block = new SourceBlock<T>();
            factory.CreateSource(block, loggingName);
            return block;
        }
    } 
}