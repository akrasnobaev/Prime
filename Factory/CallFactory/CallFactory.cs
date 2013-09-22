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
        private readonly IDictionary<string, IList<object>> _collections;

        public CallFactory()
        {
            _threads = new List<Thread>();
            _collections = new Dictionary<string, IList<object>>();
        }

        public void Start()
        {
            foreach (var thread in _threads)
                thread.Start();
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