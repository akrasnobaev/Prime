using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Prime
{
    public partial class LibertyFactory : PrimeFactoryBase
    {
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

        public LibertyFactory()
        {
            collections = new Dictionary<string, PrintableList<object>>();
            pseudoNames = new Dictionary<string, string>();
            timestamps = new Dictionary<string, List<TimeSpan>>();
        }

        public override string DumpDb()
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

        public override void ConsoleLog<T>(string InputName, PrintableList<T>.ToString ToString = null)
        {
            if (pseudoNames.ContainsKey(InputName))
                InputName = pseudoNames[InputName];

            if (!collections.ContainsKey(InputName))
                throw new PrimeException(string.Format("Not fount input wint name {0}", InputName));

            collections[InputName].Print(t => ToString((T) t));
        }
    }
}