namespace Prime
{
    public class LinkSourceToChainGenericService<TIn, TOut> : IGenericService
    {
        private readonly ISource<TIn> source;
        private readonly SourceBlock<TOut> sourceBlock;
        private ISourceReader<TIn> reader;
        private readonly IFunctionalBlock<TIn, TOut> functinalBlock;

        public LinkSourceToChainGenericService(ISource<TIn> source, IChain<TIn, TOut> chain,
            SourceBlock<TOut> sourceBlock)
        {
            this.source = source;
            this.sourceBlock = sourceBlock;
            functinalBlock = chain.ToFunctionalBlock();
        }

        public void Initialize()
        {
            reader = source.CreateReader();
        }

        public void DoWork()
        {
            while (true)
            {
                var input = reader.Get();
                var output = functinalBlock.Process(input);
                sourceBlock.Publish(output);
            }
        }
    }
}