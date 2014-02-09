using System;

namespace Prime
{
    public static class ServiceNameHelper
    {
        public static string GetInName()
        {
            return Guid.NewGuid().ToString();
        }

        public static string GetOutName()
        {
            return Guid.NewGuid().ToString();
        }

        public static string GetTimeStampName(string key)
        {
            return string.Format("{0}_timestamp", key);
        }

        /// <summary>
        /// Название для набора данных типа T. Используется для логирования.
        /// </summary>
        /// <typeparam name="T">Тип коллекции данных</typeparam>
        /// <returns>Название для коллекции данных</returns>
        public static string GetCollectionName<T>()
        {
            return typeof(T).Name + '_' + Guid.NewGuid();
        }
    }
}