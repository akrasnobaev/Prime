using System.Collections.Generic;
using System.Threading;
using OptimusPrime.Factory;

namespace OptimusPrime.Templates
{
    public class CallSource<T> : ICallSource<T>
    {
        public IFactory Factory { get; private set; }

        public CallSource(CallFactory factory)
        {
            Factory = factory;
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