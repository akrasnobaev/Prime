using System;

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
            if (input is ICloneable)
                return (T) ((ICloneable) input).Clone();
            return input;
        }
    }
}