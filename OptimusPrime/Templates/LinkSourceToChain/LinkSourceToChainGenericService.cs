namespace Prime
{
    public class LinkSourceToChainGenericService<TIn, TOut> : IGenericService
    {
        private readonly IReciever<TIn> source;
        private readonly EventBlock<TOut> eventBlock;
        private IReader<TIn> reader;
        private readonly IFunctionalBlock<TIn, TOut> functinalBlock;

        public LinkSourceToChainGenericService(ISource<TIn> source, IChain<TIn, TOut> chain,
            EventBlock<TOut> eventBlock)
        {
            this.source = source.Factory.CreateReciever(source);
            this.eventBlock = eventBlock;
            functinalBlock = chain.ToFunctionalBlock();
        }

        public void Initialize()
        {
            reader = source.GetReader();
        }

        public void DoWork()
        {
            while (true)
            {
                var input = reader.Get();
                var output = functinalBlock.Process(input);
                eventBlock.Publish(output);
            }
        }
    }
}