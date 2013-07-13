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

        public ISourceReader<TPublic> CreateReader()
        {
            return new OptimusPrimeReader<TPublic>(Output.Name);
        }
    }
}