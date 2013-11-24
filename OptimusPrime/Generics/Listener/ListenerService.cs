using OptimusPrime.Templates;
using System;

namespace OptimusPrime.Generics
{
    public class ListenerService<T> : IGenericService 
    {
        ISource<T> input;
        ISourceReader<T> reader;
        Action<T> action;
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
