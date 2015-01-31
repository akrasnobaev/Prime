using System.Collections.Generic;

namespace Prime.Liberty
{
    public class LibertySource<T> : ILibertySource<T>
    {
        public IPrimeFactory Factory { get; private set; }
        public string Name { get; private set; }
        public PrintableList<object> Collection { get; private set; }

        public IList<LibertyReciever<T>> Recievers { get; private set; }

        public LibertySource(LibertyFactory factory, string name)
        {
            Factory = factory;
            Name = name;
            Collection = new PrintableList<object>();
            Recievers = new List<LibertyReciever<T>>();
        }

        public IReciever<T> CreateReciever(string readLogName = null)
        {
            return Factory.CreateReciever(this, readLogName);
        }

        public void Release()
        {
            lock (Recievers)
                foreach (var reciever in Recievers)
                    reciever.AvailableData.Release();
        }
    }
}