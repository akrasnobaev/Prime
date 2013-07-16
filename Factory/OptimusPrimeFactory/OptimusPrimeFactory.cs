using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookSleeve;
using OptimusPrime.OprimusPrimeCore;

namespace OptimusPrime.Factory
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
            var connection = new RedisConnection("localhost", allowAdmin: true);

            Task openTask = connection.Open();
            connection.Wait(openTask);

            Task flushDbTask = connection.Server.FlushAll();
            connection.Wait(flushDbTask);

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