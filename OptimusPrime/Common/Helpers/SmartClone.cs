using System;
using System.ComponentModel;

namespace Prime
{
    public class SmartClone<T>
    {
        public readonly bool IsEmptyCloning = false;

        public SmartClone()
        {
            // Если объект неизменяемый - передаем данные как есть
            var immutableAttribute =
                (ImmutableObjectAttribute) Attribute.GetCustomAttribute(typeof (T), typeof (ImmutableObjectAttribute));
            if (immutableAttribute != null && immutableAttribute.Immutable)
            {
                copyFunc = input => input;
                IsEmptyCloning = true;
            }

                // Если данные можно клонировать - клонируем.
            else if (typeof(ICloneable).IsAssignableFrom(typeof(T)))
                copyFunc = input => (T)((ICloneable)input).Clone();

                // Если данные можно сериализовать - сериализуем
            else if (typeof(T).IsSerializable)
                copyFunc = input => input.Serialize().Deserialize<T>();

                // В противном случае выбрасываем исключение
            else
                throw new DataCanNotBeClonnedPrimeException(typeof(T));
        }

        /// <summary>
        /// Способ копирования данных.
        /// </summary>
        private readonly Func<T, T> copyFunc;

        public T Clone(T input)
        {
            return copyFunc(input);
        }
    }
}