namespace Prime
{
    public class LinkSourceToChainGenericService<TIn, TOut> : IGenericService
    {
        private readonly ISource<TIn> source;
        private readonly IChain<TIn, TOut> chain;
        private readonly SourceBlock<TOut> sourceBlock;
        private ISourceReader<TIn> reader;
        private IFunctionalBlock<TIn, TOut> functinalBlock;

        public LinkSourceToChainGenericService(ISource<TIn> source, IChain<TIn, TOut> chain, SourceBlock<TOut> sourceBlock)
        {
            this.source = source;
            this.chain = chain;
            this.sourceBlock = sourceBlock;
        }

        public void Initialize()
        {
            reader = source.CreateReader();
            functinalBlock = chain.ToFunctionalBlock();
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