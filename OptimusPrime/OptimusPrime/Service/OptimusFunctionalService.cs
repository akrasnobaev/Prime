using System;
using System.Diagnostics;

namespace Prime.Optimus
{
    public class OptimusFunctionalService<TIn, TOut> : OptimusService
    {
        private readonly Func<TIn, TOut> functionalBlock;

        public IOptimusIn Input
        {
            get { return OptimusIn[0]; }
        }

        public IOptimusOut Output
        {
            get { return OptimusOut[0]; }
        }

        public OptimusFunctionalService(Func<TIn, TOut> functionalBlock, string inputKey, string outputKey,
            Stopwatch stopwatch, bool isLogging)
        {
            this.functionalBlock = functionalBlock;

            OptimusIn = new IOptimusIn[] {new OptimusIn(inputKey, this)};
            OptimusOut = new IOptimusOut[] {new OptimusOut(outputKey, this, stopwatch, isLogging)};
        }

        public override void Initialize()
        {
        }

        public override void DoWork()
        {
            while (true)
            {
                var input = Input.Get<TIn>();
                var output = functionalBlock(input);
                Output.Set(output);
            }
        }
    }
}