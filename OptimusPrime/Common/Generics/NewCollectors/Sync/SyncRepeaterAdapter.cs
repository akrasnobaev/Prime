namespace Prime
{
    public class SyncRepeaterAdapter<T> : IFunctionalBlock<CollectorRequest, T>
    {
        private T storedData = default(T);

        private bool pushedBack = true;

        private IFunctionalBlock<Token, T> Collector;

        public SyncRepeaterAdapter(IFunctionalBlock<Token, T> collector)
        {
            Collector = collector;
        }

        public T Process(CollectorRequest input)
        {
            if (input == CollectorRequest.Pushbask)
            {
                pushedBack = true;
                return default(T);
            }
            if (pushedBack)
                pushedBack = false;
            else
                storedData = Collector.Process(Token.Empty);
            return storedData;
        }
    }
}