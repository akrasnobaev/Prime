using OptimusPrime.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptimusPrime.Generics
{
    public class SyncRepeaterAdapter<T> : IFunctionalBlock<CollectorRequest, T>
    {
        T storedData = default(T);

        bool pushedBack = true;

        IFunctionalBlock<Token, T> Collector;

        public SyncRepeaterAdapter(IFunctionalBlock<Token, T> collector)
        {
            Collector = collector;
        }

        public T Process(CollectorRequest input)
        {
            if (input == CollectorRequest.Pushbask)
            {
                pushedBack = true;
                return default(T);
            }
            if (pushedBack)
                pushedBack = false;
            else
                storedData = Collector.Process(Token.Empty);
            return storedData;
        }
    }
}