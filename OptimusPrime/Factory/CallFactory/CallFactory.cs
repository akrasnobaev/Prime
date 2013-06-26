using System.Collections.Generic;
using System.Threading;

namespace OptimusPrime.Factory.CallFactory
{
    public partial class CallFactory : IFactory
    {
        private readonly IList<Thread> threads;

        public CallFactory()
        {
            threads = new List<Thread>();
        }

        public void Start()
        {
            foreach (var thread in threads)
                thread.Start();
        }

        public void Stop()
        {
            foreach (var thread in threads)
                thread.Abort();
        }
    }
}