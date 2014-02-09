using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prime
{
    public static partial class FactoryExtensions
    {
        public static IChain<TIn[], TOut[]> Foreach<TIn, TOut>
      (
      this IPrimeFactory factory,
      IChain<TIn, TOut> privateChaine)
        {
            var block = privateChaine.ToFunctionalBlock();
            return factory.CreateChain<TIn[], TOut[]>(
                z =>
                {
                    var res = new TOut[z.Length];
                    for (int i = 0; i < z.Length; i++)
                        res[i] = block.Process(z[i]);
                    return res;
                });
        }
    }
}