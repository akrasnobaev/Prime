using OptimusPrime.OprimusPrimeCore;

namespace OptimusPrime.Templates
{
    public class OptimusPrimeChain<TIn, TOut> : IOptimusPrimeChane<TIn, TOut>
    {
        public OptimusPrimeChain(IOptimusPrimeIn input, IOptimusPrimeOut output)
        {
            Input = input;
            Output = output;
        }

        public IOptimusPrimeIn Input { get; private set; }
        public IOptimusPrimeOut Output { get; private set; }

        public IFunctionalBlock<TIn, TOut> ToFunctionalBlock()
        {
            var inputService = new OptimusPrimeStabService();
            inputService.OptimusPrimeOut = new IOptimusPrimeOut[] {new OptimusPrimeOut(Input.Name, inputService)};

            var outputService = new OptimusPrimeStabService();
            outputService.OptimusPrimeIn = new IOptimusPrimeIn[] {new OptimusPrimeIn(Output.Name, outputService)};

            return new FunctionalBlock<TIn, TOut>(value =>
                {
                    inputService.OptimusPrimeOut[0].Set(value);
                    return outputService.OptimusPrimeIn[0].Get<TOut>();
                });
        }
    }
}