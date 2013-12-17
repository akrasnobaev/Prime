using OptimusPrime.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OptimusPrime.Factory;
using OptimusPrime.Generics;

namespace OptimusPrime.Generics
{
    public class AsyncCollector<T> 
    {
        public readonly IChain<Token, T[]> CollectorChain;

        public AsyncCollector(IChain<Token, T[]> collectorChain)
        {
            CollectorChain = collectorChain;
        }

        public IChain<CollectorRequest, T[]> CreateRepeaterAdapter(string pseudoName = null)
        {
            return CollectorChain.Factory.CreateChain<CollectorRequest,T[]>(
                new AsyncRepeaterAdapter<T>(
                    CollectorChain.ToFunctionalBlock()),
                    pseudoName);
        }
    }
}
