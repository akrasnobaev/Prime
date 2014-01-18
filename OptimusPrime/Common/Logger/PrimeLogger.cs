using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OptimusPrime.Common.Exception;

namespace Prime
{
    public class PrimeLogger : IPrimeLogger
    {
        private Dictionary<string, object[]> _data;
        private Dictionary<string, int> _readCounter;
        private Dictionary<string, string> _pseudoNames;
        private Dictionary<string, List<TimeSpan>> _timestamps;

        //TODO: зачем здесь пустой конструктор? Он вводит в заблуждение, потому что без вызова дополнительных методов 
        //класс не работоспособен
        public PrimeLogger()
        {
        }

        //TODO: добавил такой конструктор, но это какая-то хрень
        //Разве нельзя получить данные без сохранения на диск?
        public PrimeLogger(IPrimeFactory factory)
        {
            if (!LoadFile(factory.DumpDb()))
                throw new Exception("Логгер не смог инициализироваться");
        }

        public PrimeLogger(string fileName)
        {
            if (!LoadFile(fileName))
                throw new Exception("Логгер не смог инициализироваться");
        }


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
            var logData = file.Deserialize<LogData>();

            _data = logData.Data;
            _pseudoNames = logData.PseudoNames;
            _timestamps = logData.Timestamps;
            _readCounter = new Dictionary<string, int>();

            foreach (var set in _data)
                _readCounter.Add(set.Key, 0);

            return true;
        }

        /// <summary>
        /// Возвращает данные типа T по ключу key.
        /// </summary>
        /// <typeparam name="T">Тип запрашиваемых данных.</typeparam>
        /// <param name="key">Имя или псевдоним коллекции данных.</param>
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
        /// <param name="key">Имя или псевдоним коллекции данных.</param>
        /// <returns>Коллекция данных типа T.</returns>
        public IEnumerable<T> GetRange<T>(string key)
        {
            // В случае обращения по псевдониму, берем значение из словаря псевдонимов.
            if (_pseudoNames.ContainsKey(key))
                key = _pseudoNames[key];

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
        /// <param name="key">Имя или псевдоним коллекции данных.</param>
        /// <param name="result">Данные типа T.</param>
        /// <returns>Результат запроса.</returns>
        public bool TryGet<T>(string key, out T result)
            where T : class
        {
            string message;
            return tryGet(key, out result, out message);
        }

        public Tuple<T, TimeSpan> GetWithTimeSpan<T>(string key) where T : class
        {
            string message;
            Tuple<T, TimeSpan> result;

            if (!tryGetWithTimeSpan(key, out result, out message))
                throw new LoggerException(message);

            return result;
        }

        public IEnumerable<Tuple<T, TimeSpan>> GetRangeWithTimeSpan<T>(string key) where T : class
        {
            // В случае обращения по псевдониму, берем значение из словаря псевдонимов.
            if (_pseudoNames.ContainsKey(key))
                key = _pseudoNames[key];

            if (!_data.ContainsKey(key))
                throw new LoggerException(string.Format("Данные по ключу '{0}' отсутствуют", key));

            if (!_timestamps.ContainsKey(key))
                throw new LoggerException(string.Format(
                    "Отпечаток времени данных типа '{0}' по ключу '{1}' отсутствуют",
                    typeof (T).Name, key));


            var datas = _data[key].Skip(_readCounter[key]).Cast<T>().ToArray();
            var timeSpans = _timestamps[key].Skip(_readCounter[key]).ToArray();

            if (datas.Length != timeSpans.Length)
                throw new LoggerException(
                    string.Format("Данных типа '{0}' и отпечатков времени по ключу '{1}' различное количество",
                        typeof (T).Name, key));

            var result = new List<Tuple<T, TimeSpan>>();
            for (int i = 0; i < datas.Length; i++)
            {
                _readCounter[key]++;
                result.Add(new Tuple<T, TimeSpan>(datas[i], timeSpans[i]));
            }
            return result;
        }

        public bool TryGetWithTimeSpan<T>(string key, out Tuple<T, TimeSpan> result) where T : class
        {
            string message;
            return tryGetWithTimeSpan(key, out result, out message);
        }

        /// <summary>
        /// Запрос на данные типа T по ключу key.
        /// </summary>
        /// <typeparam name="T">Тип запрашиваемых данных.</typeparam>
        /// <param name="key">Имя или псевдоним коллекции данных.</param>
        /// <param name="result">Данные типа T.</param>
        /// <param name="message">Сообщение об ошибке. Пусто, если значение получено без ошибок.</param>
        /// <returns>Результат запроса</returns>
        private bool tryGet<T>(string key, out T result, out string message)
            where T : class
        {
            result = default(T);
            message = "";

            // В случае обращения по псевдониму, берем значение из словаря псевдонимов.
            if (_pseudoNames.ContainsKey(key))
                key = _pseudoNames[key];

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

        private bool tryGetWithTimeSpan<T>(string key, out Tuple<T, TimeSpan> result, out string message)
            where T : class
        {
            result = new Tuple<T, TimeSpan>(default(T), default(TimeSpan));
            message = "";

            // В случае обращения по псевдониму, берем значение из словаря псевдонимов.
            if (_pseudoNames.ContainsKey(key))
                key = _pseudoNames[key];

            if (!_data.ContainsKey(key))
            {
                message = string.Format("Данные по ключу '{0}' отсутствуют", key);
                return false;
            }

            if (!_timestamps.ContainsKey(key))
            {
                message = string.Format("Отпечаток времени данных типа '{0}' по ключу '{1}' отсутствуют",
                    typeof (T).Name, key);
                return false;
            }

            if (_data[key].Length <= _readCounter[key])
            {
                message = string.Format("Данные по ключу '{0}' закончились", key);
                return false;
            }

            if (_timestamps[key].Count <= _readCounter[key])
            {
                message = string.Format("Отпечатки времени данных типа '{0}' по ключу '{1}' закончились",
                    typeof (T).Name, key);
                return false;
            }

            var data = _data[key][_readCounter[key]];
            if (!(data is T))
            {
                message = string.Format("Значение по ключу {0} не может быть приведено к типу {1}", key, typeof (T).Name);
                return false;
            }
            result = new Tuple<T, TimeSpan>(data as T, _timestamps[key][_readCounter[key]]);

            _readCounter[key]++;
            return true;
        }
    }
}