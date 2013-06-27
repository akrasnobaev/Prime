using OptimusPrime.OprimusPrimeCore.Service;

namespace OptimusPrime.Templates.TopologyItems.Chain.OptimusPrime
{
    public class OptimusPrimeChain<TIn, TOut> : IOptimusPrimeChane<TIn, TOut>
    {
        public IOptimusPrimeIn Input { get; private set; }
        public IOptimusPrimeOut Output { get; private set; }

        public OptimusPrimeChain(IOptimusPrimeIn input, IOptimusPrimeOut output)
        {
            Input = input;
            Output = output;
        }
    }
}