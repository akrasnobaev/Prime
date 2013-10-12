using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookSleeve;
using OptimusPrime.OprimusPrimeCore;
using OptimusPrime.OprimusPrimeCore.Extension;
using OptimusPrime.OprimusPrimeCore.Helpers;

namespace OptimusPrime.Factory
{
    public partial class OptimusPrimeFactory : IFactory
    {
        private readonly List<Thread> threads;
        private RedisConnection connection;
        private static IList<IOptimusPrimeService> Services { get; set; }
        

        public OptimusPrimeFactory()
        {
            Services = new List<IOptimusPrimeService>();
            threads = new List<Thread>();
        }

        public void Start()
        {
            connection = new RedisConnection("localhost", allowAdmin: true);

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

        public string DumpDb()
        {
            var db = new Dictionary<string, object[]>();
            foreach (var service in Services)
                foreach (var output in service.OptimusPrimeOut)
                {
                    var range = connection.Lists.Range(output.Service.DbPage, output.Name, 0, -1);
                    var bytes = connection.Wait(range);
                    var result = bytes.Select(SerializeExtension.Deserialize<object>).ToArray();

                    db.Add(output.Name, result);
                }
            var filePath = PathHelper.GetFilePath();
            File.WriteAllBytes(filePath, db.Serialize());
            return filePath;
        }
    }
}