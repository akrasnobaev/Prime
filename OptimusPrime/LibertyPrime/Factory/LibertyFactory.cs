using System;
using System.Collections.Generic;
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
        private readonly IList<Thread> _threads;

        /// <summary>
        /// Коллекция AutoResetEvent, которые релизятся в тот момент,
        /// когда соответствующий поток успешно стартовал.
        /// </summary>
        private readonly IList<AutoResetEvent> _threadsStartSuccessed;

        /// <summary>
        /// Список коллекций данных, порожденных топологическими единицами.
        /// </summary>
        private readonly IDictionary<string, PrintableList<object>> _collections;

        /// <summary>
        /// Коллекция псевдонимов имен.
        /// </summary>
        private readonly Dictionary<string, string> _pseudoNames;

        public LibertyFactory()
        {
            _threads = new List<Thread>();
            _threadsStartSuccessed = new List<AutoResetEvent>();
            _collections = new Dictionary<string, PrintableList<object>>();
            _pseudoNames = new Dictionary<string, string>();
        }

        public void Start()
        {
            foreach (var thread in _threads)
                thread.Start();

            // Ожидание того, что все потоки стартовали успешно.
            foreach (var resetEvent in _threadsStartSuccessed)
                resetEvent.WaitOne();
        }

        public void Stop()
        {
            foreach (var thread in _threads)
                thread.Abort();
        }

        public string DumpDb()
        {
            var filePath = PathHelper.GetFilePath();
            var serialozableCollections = _collections.ToDictionary(
                collection => collection.Key,
                collection => collection.Value.ToArray());
            var logData = new LogData(_pseudoNames, serialozableCollections);
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

            _threads.Add(newSourceThread);
            _threadsStartSuccessed.Add(startSuccesed);
        }

        public void ConsoleLog<T>(string InputName, PrintableList<T>.ToString ToString = null)
        {
            if (_pseudoNames.ContainsKey(InputName))
                InputName = _pseudoNames[InputName];

            if (!_collections.ContainsKey(InputName))
                throw new PrimeException(string.Format("Not fount input wint name {0}", InputName));

            _collections[InputName].Print(t => ToString((T) t));
        }

        /// <summary>
        /// Название для набора данных типа T. Используется для логирования.
        /// TODO: Подумать как удобнее это делать.
        /// </summary>
        /// <typeparam name="T">Тип коллекции данных</typeparam>
        /// <returns>Название для коллекции данных</returns>
        private string GetCollectionName<T>()
        {
            return typeof (T).Name + '_' + Guid.NewGuid();
        }
    }
}