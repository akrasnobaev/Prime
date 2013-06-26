using System;

namespace OptimusPrime.Templates.FunctionalItems.FunctionalBlock
{
    public class OptimusPrimeEventArgs<TData> : EventArgs
    {
        public OptimusPrimeEventArgs(TData data)
        {
            Data = data;
        }

        public TData Data { get; private set; }
    }
}