namespace Prime
{
    public class SyncCollector<T>
    {
        public readonly IChain<Token, T> CollectorChain;

        public SyncCollector(IChain<Token, T> collectorChain)
        {
            CollectorChain = collectorChain;
        }

        public IChain<CollectorRequest, T> CreateRepeaterAdapter(string pseudoName = null)
        {
            return CollectorChain.Factory.CreateChain<CollectorRequest, T>(
                new SyncRepeaterAdapter<T>(
                    CollectorChain.ToFunctionalBlock()),
                pseudoName);
        }
    }
}