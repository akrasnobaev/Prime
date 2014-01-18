using System;
using System.Collections.Generic;

namespace Prime
{
    [Serializable]
    public class LogData
    {
        /// <summary>
        /// Коллекция псекдонимов имен.
        /// </summary>
        public Dictionary<string, string> PseudoNames { get; private set; }

        /// <summary>
        /// Список коллекций данных.
        /// </summary>
        public Dictionary<string, object[]> Data { get; private set; }

        /// <summary>
        /// Список коллекций отпечатков времени создания данных.
        /// </summary>
        public Dictionary<string, List<TimeSpan>> Timestamps { get; set; }

        public LogData(Dictionary<string, string> pseudoNames, Dictionary<string, object[]> data,
            Dictionary<string, List<TimeSpan>> timestamps)
        {
            PseudoNames = pseudoNames;
            Data = data;
            Timestamps = timestamps;
        }
    }
}