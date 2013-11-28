using OptimusPrime.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptimusPrime.Factory
{
    public static partial class ChainExtension
    {
        public static Isolator<TIn, TOut> Isolate<TIn, TOut>(this IChain<TIn, TOut> chain)
        {
            return new Isolator<TIn, TOut>(chain);
        }
    }
}
