using System.Diagnostics;

namespace Prime.Optimus
{
    public class OptimusSourceService<TData> : OptimusService
    {
        private readonly ISourceBlock<TData> sourceBlock;

        public IOptimusOut Output
        {
            get { return OptimusOut[0]; }
        }

        public OptimusSourceService(ISourceBlock<TData> sourceBlock, string outputName, Stopwatch stopwatch,
            bool isLogging)
        {
            this.sourceBlock = sourceBlock;

            OptimusOut = new IOptimusOut[] {new OptimusOut(outputName, this, stopwatch, isLogging)};
            OptimusIn = new IOptimusIn[0];
        }

        public override void Initialize()
        {
            sourceBlock.Event += (sender, e) => Output.Set(e);
        }

        public override void DoWork()
        {
        }
    }
}