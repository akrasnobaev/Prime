using System;

namespace OptimusPrime.Templates
{
    public class CallChain<T1, T2> : ICallChain<T1, T2>
    {
        public Func<T1, T2> Action { get; private set; } 

        public CallChain(Func<T1, T2> action)
        {
            Action = action;
        }

        public ICallChain<T1, T2> ToCallChain()
        {
            return this;
        }
    }
}