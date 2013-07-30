using OptimusPrime.OprimusPrimeCore;

namespace OptimusPrime.Templates
{
    public class OptimusPrimeSourceService<TData> : OptimusPrimeService
    {
        public IOptimusPrimeOut Output { get { return OptimusPrimeOut[0]; } }

        public OptimusPrimeSourceService(ISourceBlock<TData> sourceBlock, string outputName)
        {
            OptimusPrimeOut = new IOptimusPrimeOut[] {new OptimusPrimeOut(outputName, this)};
            OptimusPrimeIn = new IOptimusPrimeIn[0];
            sourceBlock.Event += (sender, e) => Output.Set(e);
        }

        public override void Actuation()
        {
        }
    }
}