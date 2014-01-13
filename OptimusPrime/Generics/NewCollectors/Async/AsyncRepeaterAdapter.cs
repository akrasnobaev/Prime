using System.Linq;

namespace Prime
{
    public class AsyncRepeaterAdapter<T> : IFunctionalBlock<CollectorRequest, T[]>
    {
        private T[] storedData = new T[0];

        private bool pushedBack = true;

        private IFunctionalBlock<Token, T[]> Collector;

        public AsyncRepeaterAdapter(IFunctionalBlock<Token, T[]> collector)
        {
            Collector = collector;
        }

        public T[] Process(CollectorRequest input)
        {
            if (input == CollectorRequest.Pushbask)
            {
                pushedBack = true;
                return new T[0];
            }
            var data = Collector.Process(Token.Empty);
            if (pushedBack)
            {
                data = storedData.Concat(data).ToArray();
                pushedBack = false;
            }
            storedData = data;
            return data;
        }
    }
}