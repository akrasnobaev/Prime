using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Prime
{
    public partial class LibertyFactory : IPrimeFactory
    {
        /// <summary>
        /// Коллекция потоков, которые запускают все источники данных.
        /// </summary>
        private readonly IList<Thread> threads;

        /// <summary>
        /// Коллекция AutoResetEvent, которые релизятся в тот момент,
        /// когда соответствующий поток успешно стартовал.
        /// </summary>
        private readonly IList<AutoResetEvent> threadsStartSuccessed;

        /// <summary>
        /// Список коллекций данных, порожденных топологическими единицами.
        /// </summary>
        private readonly IDictionary<string, PrintableList<object>> collections;

        /// <summary>
        /// Коллекция псевдонимов имен.
        /// </summary>
        private readonly Dictionary<string, string> pseudoNames;

        /// <summary>
        /// Список коллекций отпечатков времени от старта фобрики, который определяет момент возникновения данных.
        /// </summary>
        private readonly Dictionary<string, List<TimeSpan>> timestamps;

        /// <summary>
        /// Секундомер, стартующий вместе со стартом фабрики
        /// </summary>
        public Stopwatch Stopwatch { get; private set; }

        public LibertyFactory()
        {
            threads = new List<Thread>();
            threadsStartSuccessed = new List<AutoResetEvent>();
            collections = new Dictionary<string, PrintableList<object>>();
            pseudoNames = new Dictionary<string, string>();
            timestamps = new Dictionary<string, List<TimeSpan>>();
            Stopwatch = new Stopwatch();
        }

        public void Start()
        {
            // Стартуем секундомер, определяющий момент возникновения данных.
            Stopwatch.Start();

            foreach (var thread in threads)
                thread.Start();

            // Ожидание того, что все потоки стартовали успешно.
            foreach (var resetEvent in threadsStartSuccessed)
                resetEvent.WaitOne();
        }

        public void Stop()
        {
            foreach (var thread in threads)
                thread.Abort();

            Stopwatch.Stop();
        }

        public string DumpDb()
        {
            var filePath = PathHelper.GetFilePath();
            var serialozableDataCollections = collections.ToDictionary(
                collection => collection.Key,
                collection => collection.Value.ToArray());
            var logData = new LogData(pseudoNames, serialozableDataCollections, timestamps);
            var data = logData.Serialize();
            File.WriteAllBytes(filePath, data);
            return filePath;
        }

        public void RegisterGenericService(IGenericService service)
        {
            var startSuccesed = new AutoResetEvent(false);

            var newSourceThread = new Thread(() =>
            {
                service.Initialize();
                startSuccesed.Set();
                service.DoWork();
            });

            threads.Add(newSourceThread);
            threadsStartSuccessed.Add(startSuccesed);
        }

        public void ConsoleLog<T>(string InputName, PrintableList<T>.ToString ToString = null)
        {
            if (pseudoNames.ContainsKey(InputName))
                InputName = pseudoNames[InputName];

            if (!collections.ContainsKey(InputName))
                throw new PrimeException(string.Format("Not fount input wint name {0}", InputName));

            collections[InputName].Print(t => ToString((T) t));
        }

        /// <summary>
        /// Название для набора данных типа T. Используется для логирования.
        /// TODO: Подумать как удобнее это делать.
        /// </summary>
        /// <typeparam name="T">Тип коллекции данных</typeparam>
        /// <returns>Название для коллекции данных</returns>
        private static string GetCollectionName<T>()
        {
            return typeof (T).Name + '_' + Guid.NewGuid();
        }
    }
}