using System;

namespace Prime
{
    public class ListenerService<T> : IGenericService
    {
        private IReciever<T> input;
        private IReader<T> reader;
        private Action<T> action;

        public ListenerService(ISource<T> input, Action<T> action)
        {
            this.input = input.Factory.CreateReciever(input);
            this.action = action;
        }

        public void Initialize()
        {
            reader = input.GetReader();
        }

        public void DoWork()
        {
            while (true)
                action(reader.Get());
        }
    }
}