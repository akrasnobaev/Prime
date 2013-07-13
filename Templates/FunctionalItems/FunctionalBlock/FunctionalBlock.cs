using System;

namespace OptimusPrime.Templates
{
    public class FunctionalBlock<TIn, TOut> : IFunctionalBlock<TIn, TOut>
    {
        private readonly Func<TIn, TOut> process;

        public FunctionalBlock(Func<TIn, TOut> process)
        {
            this.process = process;
        }

        public TOut Process(TIn input)
        {
            return process(input);
        }
    }
}