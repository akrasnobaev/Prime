using System;

namespace Prime.Optimus
{
    public class MockOptimusReciever<T> : IOptimusReciever<T>
    {
        public MockOptimusReciever(string readLog)
        {
            Input = new OptimusIn(readLog, new OptimusStabService());
            InputName = readLog;
        }

        public IPrimeFactory Factory
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        public IReader<T> GetReader()
        {
            return new OptimusReader<T>(this);
        }

        public IOptimusIn Input { get; private set; }
        public T Get()
        {
            Fetch();
            return currentBuffer[0];
        }

        public bool TryGet(out T data)
        {
            Fetch();
            if (currentBuffer != null && currentBuffer.Length > 0)
            {
                data = currentBuffer[0];
                return true;
            }

            data = default(T);
            return false;
        }

        public T[] GetCollection()
        {
            Fetch();
            return currentBuffer;
        }

        public string InputName { get; private set; }

        private void Fetch()
        {
            currentBuffer = Input.Get<T[]>();
        }

        private T[] currentBuffer;
    }
}
