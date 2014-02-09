using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prime
{
    public static partial class FactoryExtensions
    {
        public static IChain<TExternalInput, TExternalOutput>
            Link<TExternalInput, TExternalOutput, TMiddle>
            (this IChain<TExternalInput, TMiddle> firstChain, Func<TMiddle, TExternalOutput> lambda,
                string pseudoName = null)
        {
            return
                firstChain.Link(firstChain.Factory.CreateChain(new Func<TMiddle, TExternalOutput>(lambda), pseudoName));
        }


        public static IChain<TExternalInput, TExternalOutput>
            Link<TExternalInput, TExternalOutput, TMiddle>
            (this IChain<TExternalInput, TMiddle> firstChain, IChain<TMiddle, TExternalOutput> secondChain)
        {
            return firstChain.Factory.LinkChainToChain(firstChain, secondChain);
        }

        public static IChain<TExternalInput, TExternalOutput>
            Link<TExternalInput, TExternalOutput, TMiddle>
            (this IChain<TExternalInput, TMiddle> firstChain, IFunctionalBlock<TMiddle, TExternalOutput> block,
                string pseudoName = null)
        {
            return
                firstChain.Link(firstChain.Factory.CreateChain(new Func<TMiddle, TExternalOutput>(block.Process),
                    pseudoName));
        }

    }
}

