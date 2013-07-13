using OptimusPrime.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimusPrime.Factory
{
    partial class IFactoryExtensions
    {
        public static SourceCollectorHelper<T1, T2> BindSources<T1, T2>(this IFactory factory, ISource<T1> source1, ISource<T2> source2)
        {
            return new SourceCollectorHelper<T1, T2>
            {
                readers = new ISourceReader[]
                {
                    source1.CreateReader(),
                    source2.CreateReader()
                }
            };
        }
    }

    public class SourceCollectorHelper<T1, T2> : SourceCollectorHelper
    {
        public SourceCollector<Q> CreateCollector<Q>()
            where Q : SourceDataCollection<T1, T2>, new()
        {
            return new SourceCollector<Q>(readers);
        }
    }

    public class SourceDataCollection<T0, T1> : ISourceDataCollection
    {
        protected List<T0> List0;
        protected List<T1> List1;
        public int ListCount
        {
            get { return 2; }
        }
        public void Pull(int index, System.Collections.IEnumerable source)
        {
            if (index == 0)
            {
                if (List0 == null) List0 = new List<T0>();
                List0.AddRange(source.Cast<T0>());
            }
            if (index == 1)
            {
                if (List1 != null) List1 = new List<T1>();
                List1.AddRange(source.Cast<T1>());
            }

        }
    }
}
