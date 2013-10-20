using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using OptimusPrime.OprimusPrimeCore.Helpers;
using OptimusPrime.OprimusPrimeCore.Extension;

namespace OptimusPrime.Factory
{
    public partial class CallFactory : IFactory
    {
        private readonly IList<Thread> _threads;
        /// <summary>
        /// Коллекция AutoResetEvent, которые релизятся в тот момент,
        /// когда соответствующий поток успешно стартовал.
        /// </summary>
        private readonly IList<AutoResetEvent> _threadsStartSuccessed;
        private readonly IDictionary<string, IList<object>> _collections;

        public CallFactory()
        {
            _threads = new List<Thread>();
            _threadsStartSuccessed = new List<AutoResetEvent>();
            _collections = new Dictionary<string, IList<object>>();
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
            var data = serialozableCollections.Serialize();
            File.WriteAllBytes(filePath, data);
            return filePath;
        }

        /// <summary>
        /// Название для набора данных типа T. Используется для логирования.
        /// TODO: Подумать как удобнее это делать.
        /// </summary>
        /// <typeparam name="T">Тип коллекции данных</typeparam>
        /// <returns>Название для коллекции данных</returns>
        private string GetCollectionName<T>()
        {
            return typeof(T).Name + '_' + Guid.NewGuid();
        }
    }
}