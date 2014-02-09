namespace Prime
{
    public class LinkSourceToFilterGenericService<T> : IGenericService
    {
        private readonly ISource<T> source;
        private readonly IFunctionalBlock<T, bool> filterBlock;
        private readonly SourceBlock<T> sourceBlock;
        private ISourceReader<T> reader;

        public LinkSourceToFilterGenericService(ISource<T> source, IFunctionalBlock<T, bool> filterBlock,
            SourceBlock<T> sourceBlock)
        {
            this.source = source;
            this.filterBlock = filterBlock;
            this.sourceBlock = sourceBlock;
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
                var isFilter = filterBlock.Process(input);
                if (isFilter)
                    sourceBlock.Publish(input);
            }
        }
    }
}