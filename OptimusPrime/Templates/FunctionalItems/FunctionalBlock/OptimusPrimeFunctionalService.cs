using OptimusPrime.OprimusPrimeCore.Service;

namespace OptimusPrime.Templates.FunctionalItems.FunctionalBlock
{
    public class OptimusPrimeFunctionalService<TIn, TOut> : OptimusPrimeService
    {
        private readonly IFunctionalBlock<TIn, TOut> functionalBlock;
        public IOptimusPrimeIn Input { get { return OptimusPrimeIn[0]; } }
        public IOptimusPrimeOut Output { get { return OptimusPrimeOut[0]; } }

        public OptimusPrimeFunctionalService(IFunctionalBlock <TIn, TOut> functionalBlock, string inputKey, string otputKey)
        {
            this.functionalBlock = functionalBlock;
            OptimusPrimeIn = new IOptimusPrimeIn[] {new OptimusPrimeIn(inputKey, this)};
            OptimusPrimeOut = new IOptimusPrimeOut[] {new OptimusPrimeOut(otputKey, this)};
        }

        public override void Actuation()
        {
            while (true)
            {
                var input = OptimusPrimeIn[0].Get<TIn>();
                var output = functionalBlock.Action.Invoke(input);
                OptimusPrimeOut[0].Set(output);
            }
        }
    }
}