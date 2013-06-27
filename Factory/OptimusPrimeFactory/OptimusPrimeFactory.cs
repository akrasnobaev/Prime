using System.Collections.Generic;
using System.Threading;
using OptimusPrime.OprimusPrimeCore.Service;

namespace OptimusPrime.Factory.OptimusPrimeFactory
{
    public partial class OptimusPrimeFactory : IFactory
    {
        private readonly List<Thread> threads;
        private static IList<IOptimusPrimeService> Services { get; set; }

        public OptimusPrimeFactory()
        {
            Services = new List<IOptimusPrimeService>();
            threads = new List<Thread>();
        }

        public void Start()
        {
            foreach (var service in Services)
            {
                var serviceThread = new Thread(service.Actuation);
                serviceThread.Start();
                threads.Add(serviceThread);
            }
        }

        public void Stop()
        {
            foreach (var thread in threads)
                thread.Abort();
        }
    }
}