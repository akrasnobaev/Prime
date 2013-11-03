using System;

namespace OptimusPrime.Templates
{
    public interface ISourceBlock<T1>
    {
        event EventHandler<T1> Event;
    }
}