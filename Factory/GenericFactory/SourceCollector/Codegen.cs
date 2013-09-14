
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


        public static SourceCollectorHelper<T0> BindSources<T0>(this IFactory factory, ISource<T0> source0)
        {
            return new SourceCollectorHelper<T0>
            {
                readers = new ISourceReader[]
            {
                source0.CreateReader()
            }
            };
        }

        public static SourceCollectorHelper<T0, T1> BindSources<T0, T1>(this IFactory factory, ISource<T0> source0, ISource<T1> source1)
        {
            return new SourceCollectorHelper<T0, T1>
            {
                readers = new ISourceReader[]
            {
                source0.CreateReader(),source1.CreateReader()
            }
            };
        }
    }



    public class SourceCollectorHelper<T0> : SourceCollectorHelper
    {
        public SourceCollector<TOutput> CreateCollector<TOutput>()
            where TOutput : SourceDataCollection<T0>, new()
        {
            return new SourceCollector<TOutput>(readers);
        }
    }

    public class SourceCollectorHelper<T0, T1> : SourceCollectorHelper
    {
        public SourceCollector<TOutput> CreateCollector<TOutput>()
            where TOutput : SourceDataCollection<T0, T1>, new()
        {
            return new SourceCollector<TOutput>(readers);
        }
    }



    public class SourceDataCollection<T0> : ISourceDataCollection
    {
        protected List<T0> List0;

        public int ListCount
        {
            get { return 1; }
        }
        public void Pull(int index, System.Collections.IEnumerable source)
        {
            if (index == 0) { if (List0 == null) List0 = new List<T0>(); List0.AddRange(source.Cast<T0>()); }
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
            if (index == 0) { if (List0 == null) List0 = new List<T0>(); List0.AddRange(source.Cast<T0>()); }
            if (index == 1) { if (List1 == null) List1 = new List<T1>(); List1.AddRange(source.Cast<T1>()); }
        }
    }
}