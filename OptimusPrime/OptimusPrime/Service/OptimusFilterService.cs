namespace Prime.Optimus
{
    public class OptimusFilterService<T> : OptimusService
    {
        private readonly IFunctionalBlock<T, bool> filterBlock;
        private readonly OptimusIn input;
        public readonly OptimusOut Output;

        public OptimusFilterService(IFunctionalBlock<T, bool> filterBlock, string inputName,
            string outputName, string host = "localhost", int dbPage = 1) : base(host, dbPage)
        {
            this.filterBlock = filterBlock;

            input = new OptimusIn(inputName, this);
            Output = new OptimusOut(outputName, this);

            OptimusOut = new IOptimusOut[] {Output};
            OptimusIn = new IOptimusIn[] {input};
        }

        public override void Initialize()
        {
        }

        public override void DoWork()
        {
            while (true)
            {
                var data = input.Get<T>();
                if (!filterBlock.Process(data)) continue;
                Output.Set(data);
            }
        }
    }
}