using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prime
{
    public static partial class FactoryExtensions
    {
        


        public static ISource<TSecondOutput>
            Link<TFirstOutput, TSecondOutput>
            (this ISource<TFirstOutput> source, IChain<TFirstOutput, TSecondOutput> chain)
        {
            return source.Factory.LinkSourceToChain(source, chain);
        }

        public static ISource<TSecondOutput>
            Link<TFirstOutput, TSecondOutput>
            (this ISource<TFirstOutput> source, IFunctionalBlock<TFirstOutput, TSecondOutput> chain,
                string pseudoName = null)
        {
            return source.Link(chain.Process, pseudoName);
        }

        public static ISource<TSecondOutput>
            Link<TFirstOutput, TSecondOutput>
            (this ISource<TFirstOutput> source, Func<TFirstOutput, TSecondOutput> chain, string pseudoName = null)
        {
            return source.Link(source.Factory.CreateChain(new Func<TFirstOutput, TSecondOutput>(chain), pseudoName));
        }
    }
}

