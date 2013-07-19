using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BookSleeve;
using OptimusPrime.OprimusPrimeCore;
using OptimusPrime.OprimusPrimeCore.Extension;

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
            var db = new Dictionary<string, byte[]>();
            foreach (var service in Services)
                foreach (var output in service.OptimusPrimeOut)
                {
                    var exportTask = connection.Server.Export(output.Service.DbPage, output.Name);
                    var bytes = connection.Wait(exportTask);

                    db.Add(output.Name, bytes);
                }
            var filePath = PathHelper.GetFilePath();
            File.WriteAllBytes(filePath, db.Serialize());
            return filePath;
        }
    }
}