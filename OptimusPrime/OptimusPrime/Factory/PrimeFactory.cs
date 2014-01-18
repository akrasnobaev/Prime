using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookSleeve;
using Prime.Optimus;

namespace Prime
{
    public partial class PrimeFactory : IPrimeFactory
    {
        /// <summary>
        /// Коллекция потоков, содержащих все сервисы.
        /// </summary>
        private readonly List<Thread> threads;

        /// <summary>
        /// Список коллекций данных, порожденных топологическими элементами.
        /// </summary>
        private RedisConnection connection;

        /// <summary>
        /// Коллекция псевдонимов имен.
        /// </summary>
        private readonly Dictionary<string, string> pseudoNames;

        /// <summary>
        /// Коллекция сервисов.
        /// </summary>
        private static IList<IOptimusService> Services { get; set; }

        /// <summary>
        /// Коллекция AutoResetEvent, которые релизятся в тот момент,
        /// когда соответствующий поток успешно стартовал.
        /// </summary>
        private readonly IList<AutoResetEvent> threadsStartSuccessed;

        /// <summary>
        /// Секундомер, стартующий вместе со стартом фабрики
        /// </summary>
        public Stopwatch Stopwatch { get; private set; }

        public PrimeFactory()
        {
            Services = new List<IOptimusService>();
            threads = new List<Thread>();
            pseudoNames = new Dictionary<string, string>();
            threadsStartSuccessed = new List<AutoResetEvent>();
            Stopwatch = new Stopwatch();
        }

        public void Start()
        {
            connection = new RedisConnection("localhost", allowAdmin: true);

            Task openTask = connection.Open();
            connection.Wait(openTask);

            Task flushDbTask = connection.Server.FlushAll();
            connection.Wait(flushDbTask);

            // Инициализируем все сервисы и создаем исполняющие потоки для каждого сервиса.
            foreach (var service in Services)
            {
                var startSuccesed = new AutoResetEvent(false);
                var serviceThread = new Thread(() =>
                {
                    service.Initialize();
                    startSuccesed.Set();
                    service.DoWork();
                });
                threads.Add(serviceThread);
                threadsStartSuccessed.Add(startSuccesed);
            }

            // Запускаем секундомер, который определяет время создания данных.
            Stopwatch.Start();

            // Запуск исолняющий потоков для всех сервисов и добавленных Generic-серисов.
            foreach (var thread in threads)
                thread.Start();

            // Ожидание того, что все потоки стартовали успешно.
            foreach (var resetEvent in threadsStartSuccessed)
                resetEvent.WaitOne();
        }

        public void Stop()
        {
            Stopwatch.Stop();

            foreach (var thread in threads)
                thread.Abort();
        }

        public string DumpDb()
        {
            var db = new Dictionary<string, object[]>();
            var timeStampsCollection = new Dictionary<string, List<TimeSpan>>();
            foreach (var service in Services)
                foreach (var output in service.OptimusOut)
                {
                    // Логирование данных.
                    var range = connection.Lists.Range(output.Service.DbPage, output.Name, 0, -1);
                    var bytes = connection.Wait(range);
                    var result = bytes.Select(SerializeExtension.Deserialize<object>).ToArray();

                    db.Add(output.Name, result);

                    // Логирование отпечатков времени создания данных.
                    range = connection.Lists.Range(output.Service.DbPage,
                        ServiceNameHelper.GetTimeStampName(output.Name), 0, -1);
                    bytes = connection.Wait(range);
                    var timeStamps = bytes.Select(SerializeExtension.Deserialize<TimeSpan>).ToList();

                    timeStampsCollection.Add(output.Name, timeStamps);
                }
            var filePath = PathHelper.GetFilePath();
            var logData = new LogData(pseudoNames, db, timeStampsCollection);
            var data = logData.Serialize();
            File.WriteAllBytes(filePath, data);
            return filePath;
        }

        public void RegisterGenericService(IGenericService service)
        {
            var startSuccesed = new AutoResetEvent(false);
            var serviceThread = new Thread(() =>
            {
                service.Initialize();
                startSuccesed.Set();
                service.DoWork();
            });
            threads.Add(serviceThread);
            threadsStartSuccessed.Add(startSuccesed);
        }

        public void ConsoleLog<T>(string InputName, PrintableList<T>.ToString ToString = null)
        {
            throw new NotImplementedException();
        }
    }
}