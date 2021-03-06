﻿using System;
using System.Collections.Generic;

namespace Prime
{
    public interface IPrimeLogger
    {
        /// <summary>
        /// Вычитывание лог-файла.
        /// </summary>
        /// <param name="filePath">Путь до лог-файла.</param>
        /// <returns>В случае успеха - true, иначе fale</returns>
        bool LoadFile(string filePath);

        /// <summary>
        /// Возвращает данные типа T по ключу key.
        /// </summary>
        /// <typeparam name="T">Тип запрашиваемых данных.</typeparam>
        /// <param name="key">Имя или псевдоним коллекции данных.</param>
        /// <returns>Данные типа T.</returns>
        T Get<T>(string key);

        /// <summary>
        /// Возвращает коллекцию данных типа T.
        /// </summary>
        /// <typeparam name="T">Тип запрашиваемых данных.</typeparam>
        /// <param name="key">Имя или псевдоним коллекции данных.</param>
        /// <returns>Коллекция данных типа T.</returns>
        IEnumerable<T> GetRange<T>(string key);

        /// <summary>
        /// Запрос на данные типа T по ключу key. В случае успеха, возвращает true,
        /// а значение возвращается в out параметре result.
        /// </summary>
        /// <typeparam name="T">Тип запрашиваемых данных</typeparam>
        /// <param name="key">Имя или псевдоним коллекции данных.</param>
        /// <param name="result">Данные типа T.</param>
        /// <returns>Результат запроса.</returns>
        bool TryGet<T>(string key, out T result);

        /// <summary>
        /// Возвращает данные типа T и временную метку, указывающую на время создания данных.
        /// </summary>
        /// <typeparam name="T">Тип запрашиваемых данных</typeparam>
        /// <param name="key">Имя или псевдоним коллекции данных</param>
        /// <returns>Пара, данные типа T и временная метка создания данных</returns>
        Tuple<T, TimeSpan> GetWithTimeSpan<T>(string key);

        /// <summary>
        /// Возвращает коллекцию пар: данных типа T и временных отпечатков создания данных.
        /// </summary>
        /// <typeparam name="T">Тип запрашиваемых данных.</typeparam>
        /// <param name="key">Имя или псевдоним коллекции данных</param>
        /// <returns>Коллекция пар: данные типа T и временные отпечатки создания данных</returns>
        IEnumerable<Tuple<T, TimeSpan>> GetRangeWithTimeSpan<T>(string key);

        /// <summary>
        /// Проверка на наличие данных типа T с временными отпечатками. В случае успеха пара данных
        /// и отпечатков времени вернутся в параметре result.
        /// </summary>
        /// <typeparam name="T">Тип запрашиваемых данных</typeparam>
        /// <param name="key">Имя или псевдоним коллекции данных</param>
        /// <param name="result">Пара: данные типа T и временной отпечаток создания данных</param>
        /// <returns>Имеются ли данные и временной отпечаток в коллекции</returns>
        bool TryGetWithTimeSpan<T>(string key, out Tuple<T, TimeSpan> result);
    }
}