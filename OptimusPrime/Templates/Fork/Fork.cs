namespace Prime
{
    public class Fork<T, Q>
    {
        public IChain<T, Q> Chain { get; private set; }
        public ISource<Q> Source { get; private set; }

        public Fork(IChain<T, Q> chain, ISource<Q> source)
        {
            Chain = chain;
            Source = source;
        }
    }
}