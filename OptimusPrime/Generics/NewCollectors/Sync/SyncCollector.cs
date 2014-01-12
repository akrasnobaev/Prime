using OptimusPrime.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OptimusPrime.Factory;

namespace OptimusPrime.Generics
{
    public class SyncCollector<T> 
    {
        public readonly IChain<Token, T> CollectorChain;

        public SyncCollector(IChain<Token, T> collectorChain)
        {
            CollectorChain = collectorChain;
        }

        public IChain<CollectorRequest, T> CreateRepeaterAdapter(string pseudoName = null)
        {
            return CollectorChain.Factory.CreateChain<CollectorRequest,T>(
                new SyncRepeaterAdapter<T>(
                    CollectorChain.ToFunctionalBlock()),
                    pseudoName);
        }
    }
}
