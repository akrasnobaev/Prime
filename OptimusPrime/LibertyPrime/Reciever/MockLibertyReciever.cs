using System;

namespace Prime.Liberty
{
    public class MockLibertyReciever<T> : ILibertyReciever<T>
    {
        private PrintableList<object> readLog;
        private int currentBuffer = 0;
        public MockLibertyReciever(IPrimeFactory factory, string readLogName, PrintableList<object> readLog)
        {
            InputName = readLogName;
            Factory = factory;
            this.readLog = readLog;
        }

        public IPrimeFactory Factory { get; private set; }

        public IReader<T> GetReader()
        {
            return new LibertyReader<T>(this);
        }

        public string InputName { get; private set; }

        public T Get()
        {
            return (readLog[currentBuffer++] as T[])[0];
        }

        public bool TryGet(out T data)
        {
            if (currentBuffer >= readLog.Count || (readLog[currentBuffer] as T[]).Length == 0)
            {
                data = default(T);
                return false;
            }
            data = (readLog[currentBuffer++] as T[])[0];
            return true;
        }

        public T[] GetCollection()
        {
            return readLog[currentBuffer++] as T[];
        }
    }
}
