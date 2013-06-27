using System;

namespace OptimusPrime.Templates.FunctionalItems.FunctionalBlock
{
    public interface ISourceBlock<T1>
    {
        event EventHandler<OptimusPrimeEventArgs<T1>> Event;
    }
}