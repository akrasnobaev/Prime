using System;

namespace OptimusPrime.Templates
{
    public interface ISourceBlock<T1>
    {
        event EventHandler<OptimusPrimeEventArgs<T1>> Event;
    }
}