using OptimusPrime.OprimusPrimeCore;
using System;

namespace OptimusPrime.Templates
{
    public class OptimusPrimeFunctionalService<TIn, TOut> : OptimusPrimeService
    {
        private readonly Func<TIn, TOut> _functionalBlock;
        public IOptimusPrimeIn Input { get { return OptimusPrimeIn[0]; } }
        public IOptimusPrimeOut Output { get { return OptimusPrimeOut[0]; } }

        public OptimusPrimeFunctionalService(Func <TIn, TOut> functionalBlock, string inputKey, string otputKey)
        {
            _functionalBlock = functionalBlock;

            OptimusPrimeIn = new IOptimusPrimeIn[] { new OptimusPrimeIn(inputKey, this) };
            OptimusPrimeOut = new IOptimusPrimeOut[] { new OptimusPrimeOut(otputKey, this) };
        }

        public override void Initialize()
        {
        }

        public override void DoWork()
        {
            while (true)
            {
                var input = OptimusPrimeIn[0].Get<TIn>();
                var output = _functionalBlock(input);
                OptimusPrimeOut[0].Set(output);
            }
        }
    }
}