namespace Prime.Optimus
{
    public class OptimusSourceReader<T> : ISourceReader<T>
    {
        private readonly OptimusIn _input;

        public OptimusSourceReader(string storageKey)
        {
            var inputService = new OptimusStabService();
            _input = new OptimusIn(storageKey, inputService);
        }

        public T Get()
        {
            return _input.Get<T>();
        }

        public bool TryGet(out T data)
        {
            return _input.TryGet(out data);
        }

        public T[] GetCollection()
        {
            return _input.GetRange<T>();
        }
    }
}