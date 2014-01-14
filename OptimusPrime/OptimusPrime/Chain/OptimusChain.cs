namespace Prime.Optimus
{
    public class OptimusChain<TIn, TOut> : IOptimusChane<TIn, TOut>
    {
        public OptimusChain(PrimeFactory factory, IOptimusIn input, IOptimusOut output)
        {
            Input = input;
            Output = output;
            Factory = factory;
        }

        public IPrimeFactory Factory { get; private set; }

        public string InputName
        {
            get { return Input.Name; }
        }

        public string OutputName
        {
            get { return Output.Name; }
        }

        public IOptimusIn Input { get; private set; }
        public IOptimusOut Output { get; private set; }

        public IFunctionalBlock<TIn, TOut> ToFunctionalBlock()
        {
            var inputService = new OptimusStabService();
            inputService.OptimusOut = new IOptimusOut[] {new OptimusOut(Input.Name, inputService)};

            var outputService = new OptimusStabService();
            outputService.OptimusIn = new IOptimusIn[] {new OptimusIn(Output.Name, outputService)};

            return new FunctionalBlock<TIn, TOut>(value =>
            {
                inputService.OptimusOut[0].Set(value);
                return outputService.OptimusIn[0].Get<TOut>();
            });
        }
    }
}