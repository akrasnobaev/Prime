
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

        public static SourceCollectorHelper<T0, T1, T2> BindSources<T0, T1, T2>(this IFactory factory, ISource<T0> source0, ISource<T1> source1, ISource<T2> source2)
        {
            return new SourceCollectorHelper<T0, T1, T2>
            {
                readers = new ISourceReader[]
            {
                source0.CreateReader(),source1.CreateReader(),source2.CreateReader()
            }
            };
        }

        public static SourceCollectorHelper<T0, T1, T2, T3> BindSources<T0, T1, T2, T3>(this IFactory factory, ISource<T0> source0, ISource<T1> source1, ISource<T2> source2, ISource<T3> source3)
        {
            return new SourceCollectorHelper<T0, T1, T2, T3>
            {
                readers = new ISourceReader[]
            {
                source0.CreateReader(),source1.CreateReader(),source2.CreateReader(),source3.CreateReader()
            }
            };
        }

        public static SourceCollectorHelper<T0, T1, T2, T3, T4> BindSources<T0, T1, T2, T3, T4>(this IFactory factory, ISource<T0> source0, ISource<T1> source1, ISource<T2> source2, ISource<T3> source3, ISource<T4> source4)
        {
            return new SourceCollectorHelper<T0, T1, T2, T3, T4>
            {
                readers = new ISourceReader[]
            {
                source0.CreateReader(),source1.CreateReader(),source2.CreateReader(),source3.CreateReader(),source4.CreateReader()
            }
            };
        }
    }



    public class SourceCollectorHelper<T0> : SourceCollectorHelper
    {
        public AsyncSourceCollector<TOutput> CreateCollector<TOutput>()
            where TOutput : SourceDataCollection<T0>, new()
        {
            return new AsyncSourceCollector<TOutput>(readers);
        }
    }

    public class SourceCollectorHelper<T0, T1> : SourceCollectorHelper
    {
        public AsyncSourceCollector<TOutput> CreateCollector<TOutput>()
            where TOutput : SourceDataCollection<T0, T1>, new()
        {
            return new AsyncSourceCollector<TOutput>(readers);
        }
    }

    public class SourceCollectorHelper<T0, T1, T2> : SourceCollectorHelper
    {
        public AsyncSourceCollector<TOutput> CreateCollector<TOutput>()
            where TOutput : SourceDataCollection<T0, T1, T2>, new()
        {
            return new AsyncSourceCollector<TOutput>(readers);
        }
    }

    public class SourceCollectorHelper<T0, T1, T2, T3> : SourceCollectorHelper
    {
        public AsyncSourceCollector<TOutput> CreateCollector<TOutput>()
            where TOutput : SourceDataCollection<T0, T1, T2, T3>, new()
        {
            return new AsyncSourceCollector<TOutput>(readers);
        }
    }

    public class SourceCollectorHelper<T0, T1, T2, T3, T4> : SourceCollectorHelper
    {
        public AsyncSourceCollector<TOutput> CreateCollector<TOutput>()
            where TOutput : SourceDataCollection<T0, T1, T2, T3, T4>, new()
        {
            return new AsyncSourceCollector<TOutput>(readers);
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

    public class SourceDataCollection<T0, T1, T2> : ISourceDataCollection
    {
        protected List<T0> List0;
        protected List<T1> List1;
        protected List<T2> List2;

        public int ListCount
        {
            get { return 3; }
        }
        public void Pull(int index, System.Collections.IEnumerable source)
        {
            if (index == 0) { if (List0 == null) List0 = new List<T0>(); List0.AddRange(source.Cast<T0>()); }
            if (index == 1) { if (List1 == null) List1 = new List<T1>(); List1.AddRange(source.Cast<T1>()); }
            if (index == 2) { if (List2 == null) List2 = new List<T2>(); List2.AddRange(source.Cast<T2>()); }
        }
    }

    public class SourceDataCollection<T0, T1, T2, T3> : ISourceDataCollection
    {
        protected List<T0> List0;
        protected List<T1> List1;
        protected List<T2> List2;
        protected List<T3> List3;

        public int ListCount
        {
            get { return 4; }
        }
        public void Pull(int index, System.Collections.IEnumerable source)
        {
            if (index == 0) { if (List0 == null) List0 = new List<T0>(); List0.AddRange(source.Cast<T0>()); }
            if (index == 1) { if (List1 == null) List1 = new List<T1>(); List1.AddRange(source.Cast<T1>()); }
            if (index == 2) { if (List2 == null) List2 = new List<T2>(); List2.AddRange(source.Cast<T2>()); }
            if (index == 3) { if (List3 == null) List3 = new List<T3>(); List3.AddRange(source.Cast<T3>()); }
        }
    }

    public class SourceDataCollection<T0, T1, T2, T3, T4> : ISourceDataCollection
    {
        protected List<T0> List0;
        protected List<T1> List1;
        protected List<T2> List2;
        protected List<T3> List3;
        protected List<T4> List4;

        public int ListCount
        {
            get { return 5; }
        }
        public void Pull(int index, System.Collections.IEnumerable source)
        {
            if (index == 0) { if (List0 == null) List0 = new List<T0>(); List0.AddRange(source.Cast<T0>()); }
            if (index == 1) { if (List1 == null) List1 = new List<T1>(); List1.AddRange(source.Cast<T1>()); }
            if (index == 2) { if (List2 == null) List2 = new List<T2>(); List2.AddRange(source.Cast<T2>()); }
            if (index == 3) { if (List3 == null) List3 = new List<T3>(); List3.AddRange(source.Cast<T3>()); }
            if (index == 4) { if (List4 == null) List4 = new List<T4>(); List4.AddRange(source.Cast<T4>()); }
        }
    }
}