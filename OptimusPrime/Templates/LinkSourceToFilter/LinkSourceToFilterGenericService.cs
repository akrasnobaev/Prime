namespace Prime
{
    public class LinkSourceToFilterGenericService<T> : IGenericService
    {
        private readonly IReciever<T> source;
        private readonly IFunctionalBlock<T, bool> filterBlock;
        private readonly EventBlock<T> eventBlock;
        private IReader<T> reader;

        public LinkSourceToFilterGenericService(ISource<T> source, IFunctionalBlock<T, bool> filterBlock,
            EventBlock<T> eventBlock)
        {
            this.source = source.Factory.CreateReciever(source);
            this.filterBlock = filterBlock;
            this.eventBlock = eventBlock;
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
                var isFilter = filterBlock.Process(input);
                if (isFilter)
                    eventBlock.Publish(input);
            }
        }
    }
}