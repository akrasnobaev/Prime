using System.Diagnostics;

namespace Prime.Optimus
{
    public class OptimusSourceService<TData> : OptimusService
    {
        private readonly IEventBlock<TData> eventBlock;

        public IOptimusOut Output
        {
            get { return OptimusOut[0]; }
        }

        public OptimusSourceService(IEventBlock<TData> eventBlock, string outputName, Stopwatch stopwatch,
            bool isLogging)
        {
            this.eventBlock = eventBlock;

            OptimusOut = new IOptimusOut[] {new OptimusOut(outputName, this, stopwatch, isLogging)};
            OptimusIn = new IOptimusIn[0];
        }

        public override void Initialize()
        {
            eventBlock.Event += (sender, e) => Output.Set(e);
        }

        public override void DoWork()
        {
        }
    }
}