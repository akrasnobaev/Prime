namespace Prime.Optimus
{
    public class OptimusSourceService<TData> : OptimusService
    {
        private readonly ISourceBlock<TData> _sourceBlock;

        public IOptimusOut Output
        {
            get { return OptimusOut[0]; }
        }

        public OptimusSourceService(ISourceBlock<TData> sourceBlock, string outputName)
        {
            _sourceBlock = sourceBlock;

            OptimusOut = new IOptimusOut[] {new OptimusOut(outputName, this)};
            OptimusIn = new IOptimusIn[0];
        }

        public override void Initialize()
        {
            _sourceBlock.Event += (sender, e) => Output.Set(e);
        }

        public override void DoWork()
        {
        }
    }
}