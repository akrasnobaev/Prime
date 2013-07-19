using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookSleeve;
using OptimusPrime.OprimusPrimeCore.Extension;

namespace OptimusPrime.OprimusPrimeCore
{
    public class OptimusPrimeLogger : IOptimusPrimeLogger
    {
        private readonly RedisConnection connection;
        private readonly int dbPage;
        private readonly Dictionary<string, int> readCounterDictionary;

        public OptimusPrimeLogger(int DbPage = 1)
        {
            dbPage = DbPage;
            connection = new RedisConnection("localhost", allowAdmin: true);
            readCounterDictionary = new Dictionary<string, int>();

            Task openTask = connection.Open();
            connection.Wait(openTask);
        }

        public bool LoadDb(string filePath)
        {
            var flushDbTask = connection.Server.FlushAll();
            connection.Wait(flushDbTask);

            if (!File.Exists(filePath))
                return false;

            var db = File.ReadAllBytes(filePath).Deserialize<Dictionary<string, byte[]>>();
            foreach (var dbItem in db)
            {
                var importTask = connection.Server.Import(dbPage, dbItem.Key, dbItem.Value);
                connection.Wait(importTask);
            }
            readCounterDictionary.Clear();
            return true;
        }

        public T Get<T>(string storageKey)
        {
            var readCounter = GetReadCounter(storageKey);
            var task = connection.Lists.Get(dbPage, storageKey, readCounter);
            var bytes = connection.Wait(task);

            if (bytes == null)
                return default(T);

            readCounterDictionary[storageKey]++;
            return bytes.Deserialize<T>();
        }

        public IEnumerable<T> GetRange<T>(string storageKey)
        {
            var readCounter = GetReadCounter(storageKey);
            var range = connection.Lists.Range(dbPage, storageKey, readCounter, -1);
            var bytes = connection.Wait(range);

            var result = bytes.Select(SerializeExtension.Deserialize<T>);
            readCounterDictionary[storageKey] += result.Count();
            return result;
        }

        private int GetReadCounter(string storageKey)
        {
            if (!readCounterDictionary.ContainsKey(storageKey))
                readCounterDictionary.Add(storageKey, 0);
            return readCounterDictionary[storageKey];
        }
    }
}