using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimusPrime.Templates;

namespace OptimusPrime
{
    public static class IOptimusPrimeAmenities
    {
        public static IChain<TExternalInput, TExternalOutput>
            Link<TExternalInput, TExternalOutput, TMiddle>
            (this IChain<TExternalInput, TMiddle> firstChain, IChain<TMiddle, TExternalOutput> secondChain)
        {
            return firstChain.Factory.LinkChainToChain(firstChain, secondChain);
        }

        public static ISource<TSecondOutput>
            Link<TFirstOutput, TSecondOutput>
            (this ISource<TFirstOutput> source, IChain<TFirstOutput, TSecondOutput> chain)
        {
            return source.Factory.LinkSourceToChain(source, chain);
        }



        }
}
