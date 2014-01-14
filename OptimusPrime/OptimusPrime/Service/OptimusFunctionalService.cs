using System;

namespace Prime.Optimus
{
    public class OptimusFunctionalService<TIn, TOut> : OptimusService
    {
        private readonly Func<TIn, TOut> _functionalBlock;

        public IOptimusIn Input
        {
            get { return OptimusIn[0]; }
        }

        public IOptimusOut Output
        {
            get { return OptimusOut[0]; }
        }

        public OptimusFunctionalService(Func<TIn, TOut> functionalBlock, string inputKey, string otputKey)
        {
            _functionalBlock = functionalBlock;

            OptimusIn = new IOptimusIn[] {new OptimusIn(inputKey, this)};
            OptimusOut = new IOptimusOut[] {new OptimusOut(otputKey, this)};
        }

        public override void Initialize()
        {
        }

        public override void DoWork()
        {
            while (true)
            {
                var input = OptimusIn[0].Get<TIn>();
                var output = _functionalBlock(input);
                OptimusOut[0].Set(output);
            }
        }
    }
}