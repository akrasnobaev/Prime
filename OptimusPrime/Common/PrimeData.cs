using System;

namespace OptimusPrime
{
    [Serializable]
    public class PrimeData<T>
    {
        public T Data { get; private set; }
        public Exception Exception { get; private set; }

        public PrimeData(T data, Exception exception)
        {
            Data = data;
            Exception = exception;
        }

        public PrimeData(T data)
        {
            Data = data;
            Exception = null;
        }
    }
}