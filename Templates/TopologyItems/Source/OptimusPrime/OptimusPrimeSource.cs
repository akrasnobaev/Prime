using OptimusPrime.OprimusPrimeCore;

namespace OptimusPrime.Templates
{
    public class OptimusPrimeSource<TPublic> : IOptimusPrimeSource<TPublic>
    {
        public OptimusPrimeSource(IOptimusPrimeOut output)
        {
            Output = output;
        }

        public IOptimusPrimeOut Output { get; private set; }

        //TODO: create OptimusPrimeSourceReader and implement this method
        public ISourceReader<TPublic> CreateReader()
        {
            throw new System.NotImplementedException();
        }
    }
}