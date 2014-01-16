﻿using System;

namespace Prime
{
    public class SmartClone<T>
    {
        public SmartClone()
        {
            //TODO: проверка на то, что с типом можно работать
        }

        public T Clone(T input)
        {
            // Если данные можно клонировать - клонируем.
            if (input is ICloneable)
                return (T) ((ICloneable) input).Clone();

            // Если данные можно сериализовать - сериализуем
            if (typeof (T).IsSerializable)
                return input.Serialize().Deserialize<T>();

            return input;
        }
    }
}