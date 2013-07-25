using System.Collections.Generic;
using System.Threading;

namespace OptimusPrime.Templates
{
    public class CallSource<T> : ICallSource<T>
    {
        public CallSource()
        {
            Collection = new List<T>();
            Semaphore = new Semaphore(0, int.MaxValue);
        }

        ~CallSource()
        {
            Semaphore.Dispose();
        }

        public ISourceReader<T> CreateReader()
        {
            return new SourceReader<T>(this);
        }

        public IList<T> Collection { get; private set; }
        public Semaphore Semaphore { get; private set; }
    }
}