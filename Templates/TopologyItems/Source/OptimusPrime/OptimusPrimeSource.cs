using OptimusPrime.OprimusPrimeCore.Service;

namespace OptimusPrime.Templates.TopologyItems.Source.OptimusPrime
{
    public class OptimusPrimeSource<TPublic> : IOptimusPrimeSource<TPublic>
    {
        public OptimusPrimeSource(IOptimusPrimeOut output)
        {
            Output = output;
        }

        public IOptimusPrimeOut Output { get; private set; }
    }
}