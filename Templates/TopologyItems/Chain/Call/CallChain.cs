using System;
using OptimusPrime.Factory;

namespace OptimusPrime.Templates
{
    public class CallChain<T1, T2> : ICallChain<T1, T2>
    {
        public Func<T1, T2> Action { get; private set; } 

        public CallChain(CallFactory factory, Func<T1, T2> action)
        {
            Action = action;
            Factory = factory;
        }



        public IFunctionalBlock<T1, T2> ToFunctionalBlock()
        {
            return new FunctionalBlock<T1, T2>(Action);
        }


        public Factory.IFactory Factory
        {
            get;
            private set; 
        }
    }
}