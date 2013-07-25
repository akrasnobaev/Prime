using OptimusPrime.OprimusPrimeCore;

namespace OptimusPrime.Templates
{
    public class OptimusPrimeSourceService<TData> : OptimusPrimeService
    {
        private readonly ISourceBlock<TData> sourceBlock;
        public IOptimusPrimeOut Output { get { return OptimusPrimeOut[0]; } }

        public OptimusPrimeSourceService(ISourceBlock<TData> sourceBlock, string outputName)
        {
            this.sourceBlock = sourceBlock;
            OptimusPrimeOut = new IOptimusPrimeOut[] {new OptimusPrimeOut(outputName, this)};
            OptimusPrimeIn = new IOptimusPrimeIn[0];
        }

        public override void Actuation()
        {
            sourceBlock.Event += (sender, e) => Output.Set(e);
        }
    }
}