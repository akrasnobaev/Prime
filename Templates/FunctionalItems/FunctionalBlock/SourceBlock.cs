using OptimusPrime.Templates;
using System;

namespace Eurobot.Services
{
    public class SourceBlock<T> : ISourceBlock<T> 
    {
        public void Publish(T value)
        {
            if (Event != null)
                Event(this, new OptimusPrimeEventArgs<T>(value));
        }
        public event EventHandler<OptimusPrimeEventArgs<T>> Event;
    }
}
