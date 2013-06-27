using System.Collections.Generic;
using System.Threading;

namespace OptimusPrime.Templates.TopologyItems.Source.Call
{
    public interface ICallSource <T> : ISource<T>
    {
        IList<T> Collection { get; }
        AutoResetEvent AutoResetEvent { get; }
    }
}