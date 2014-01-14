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
        /// Коллекция данных.
        /// </summary>
        public Dictionary<string, object[]> Data { get; private set; }

        public LogData(Dictionary<string, string> pseudoNames, Dictionary<string, object[]> data)
        {
            PseudoNames = pseudoNames;
            Data = data;
        }
    }
}