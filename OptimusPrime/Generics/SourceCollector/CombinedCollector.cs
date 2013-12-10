using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptimusPrime.Templates
{
    public class CombinedCollector<T1,T2> : ISourceCollector<Tuple<T1,T2>>
    {
        ISourceCollector<T1> collector1;
        ISourceCollector<T2> collector2;

        public Tuple<T1, T2> Get()
        {
            var data1 = collector1.Get();
            var data2 = collector2.Get();
            return Tuple.Create(data1, data2);
        }

        public CombinedCollector(ISourceCollector<T1> collector1, ISourceCollector<T2> collector2)
        {
            this.collector1 = collector1;
            this.collector2 = collector2;
        }

        public void Reset()
        {
            collector1.Reset();
            collector2.Reset();
        }
    }

    public class CombinedCollector
    {
        public static CombinedCollector<T1, T2> Create<T1, T2>(ISourceCollector<T1> collector1, ISourceCollector<T2> collector2)
        {
            return new CombinedCollector<T1, T2>(collector1, collector2);
        }
    }
}
