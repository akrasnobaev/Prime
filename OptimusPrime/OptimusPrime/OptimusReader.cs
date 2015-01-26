namespace Prime.Optimus
{
    public class OptimusReader<T> : IReader<T>
    {
        private readonly OptimusReciever<T> _input;

        public OptimusReader(OptimusReciever<T> reciever)
        {
            _input = reciever;
        }

        public T Get()
        {
            return _input.Get();
        }

        public bool TryGet(out T data)
        {
            return _input.TryGet(out data);
        }

        public T[] GetCollection()
        {
            return _input.GetCollection();
        }
    }
}
