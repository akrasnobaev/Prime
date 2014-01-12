using OptimusPrime.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptimusPrime.Generics
{
    public class AsyncRepeaterAdapter<T> : IFunctionalBlock<CollectorRequest, T[]>
    {
        T[] storedData = new T[0];

        bool pushedBack = true;

        IFunctionalBlock<Token, T[]> Collector;

        public AsyncRepeaterAdapter(IFunctionalBlock<Token, T[]> collector)
        {
            Collector = collector;
        }

        public T[] Process(CollectorRequest input)
        {
            if (input == CollectorRequest.Pushbask)
            {
                pushedBack = true;
                return new T[0];
            }
            var data = Collector.Process(Token.Empty);
            if (pushedBack)
            {
                data = storedData.Concat(data).ToArray();
                pushedBack = false;
            }
            storedData = data;
            return data;
        }
    }

}