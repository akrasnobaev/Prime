using OptimusPrime.Factory;
using OptimusPrime.OprimusPrimeCore;

namespace OptimusPrime.Templates
{
    public class OptimusPrimeSource<TPublic> : IOptimusPrimeSource<TPublic>
    {
        public IFactory Factory { get; private set; }

        public OptimusPrimeSource(OptimusPrimeFactory factory, IOptimusPrimeOut output)
        {
            Factory = factory;
            Output = output;
        }

        public IOptimusPrimeOut Output { get; private set; }

        public ISourceReader<TPublic> CreateReader()
        {
            return new OptimusPrimeReader<TPublic>(Output.Name);
        }

        public string Name
        {
            get { return Output.Name; }
        }
    }
}