using System.Collections.Generic;
using System.IO;
using System.Linq;
using OptimusPrime.OprimusPrimeCore.Extension;

namespace OptimusPrime.OprimusPrimeCore
{
    public class OptimusPrimeLogger : IOptimusPrimeLogger
    {
        private Dictionary<string, object[]> _data;
        private Dictionary<string, int> _readCounter;

        /// <summary>
        /// Вычитывание лог-файла.
        /// </summary>
        /// <param name="filePath">Путь до лог-файла.</param>
        /// <returns>В случае успеха - true, иначе fale</returns>
        public bool LoadFile(string filePath)
        {
            if (!File.Exists(filePath))
                return false;

            var file = File.ReadAllBytes(filePath);
            _data = file.Deserialize<Dictionary<string, object[]>>();
            _readCounter = new Dictionary<string, int>();

            foreach (var set in _data)
                _readCounter.Add(set.Key, 0);

            return true;
        }

        /// <summary>
        /// Возвращает данные типа T по ключу key.
        /// </summary>
        /// <typeparam name="T">Тип запрашиваемых данных.</typeparam>
        /// <param name="key">Ключ в лог-файле, который соответствует коллекции данных.</param>
        /// <returns>Данные типа T.</returns>
        public T Get<T>(string key) where T : class
        {
            T result;
            string message;
            if (!tryGet(key, out result, out message))
                throw new LoggerException(message);

            return result;
        }

        /// <summary>
        /// Возвращает коллекцию данных типа T.
        /// </summary>
        /// <typeparam name="T">Тип запрашиваемых данных.</typeparam>
        /// <param name="key">Ключ в лог-файле, который соответствует коллекции данных.</param>
        /// <returns>Коллекция данных типа T.</returns>
        public IEnumerable<T> GetRange<T>(string key)
        {
            if (!_data.ContainsKey(key))
                throw new LoggerException(string.Format("Данные по ключу '{0}' отсутствуют", key));

            var result = _data[key].Skip(_readCounter[key]).Cast<T>();
            _readCounter[key] = _data[key].Count();
            return result;
        }

        /// <summary>
        /// Запрос на данные типа T по ключу key. В случае успеха, возвращает true,
        /// а значение возвращается в out параметре result.
        /// </summary>
        /// <typeparam name="T">Тип запрашиваемых данных</typeparam>
        /// <param name="key">Ключ в лог-файле, который соответствует коллекции данных.</param>
        /// <param name="result">Данные типа T.</param>
        /// <returns>Результат запроса.</returns>
        public bool TryGet<T>(string key, out T result)
            where T : class
        {
            string message;
            return tryGet(key, out result, out message);
        }

        /// <summary>
        /// Запрос на данные типа T по ключу key.
        /// </summary>
        /// <typeparam name="T">Тип запрашиваемых данных.</typeparam>
        /// <param name="key">Ключ в лог-файле, который соответствует коллекции данных.</param>
        /// <param name="result">Данные типа T.</param>
        /// <param name="message">Сообщение об ошибке. Пусто, если значение получено без ошибок.</param>
        /// <returns>Результат запроса</returns>
        private bool tryGet<T>(string key, out T result, out string message)
            where T : class
        {
            result = default(T);
            message = "";

            if (!_data.ContainsKey(key))
            {
                message = string.Format("Данные по ключу '{0}' отсутствуют", key);
                return false;
            }

            if (_data[key].Length <= _readCounter[key])
            {
                message = string.Format("Данные по ключу '{0}' закончились", key);
                return false;
            }

            var data = _data[key][_readCounter[key]];
            if (!(data is T))
            {
                message = string.Format("Значение по ключу {0} не может быть приведено к типу {1}", key, typeof (T).Name);
                return false;
            }

            _readCounter[key]++;
            result = data as T;
            return true;
        }
    }
}