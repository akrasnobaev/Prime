using System;

namespace Prime
{
    public class ListenerService<T> : IGenericService
    {
        private ISource<T> input;
        private ISourceReader<T> reader;
        private Action<T> action;

        public ListenerService(ISource<T> input, Action<T> action)
        {
            this.input = input;
            this.action = action;
        }

        public void Initialize()
        {
            reader = input.CreateReader();
        }

        public void DoWork()
        {
            while (true)
                action(reader.Get());
        }
    }
}