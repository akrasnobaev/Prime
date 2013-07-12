using System.Collections.Generic;
using System.Threading;

namespace OptimusPrime.Templates
{
    public interface ICallSource <T> : ISource<T>
    {
        IList<T> Collection { get; }
        AutoResetEvent AutoResetEvent { get; }
    }
}