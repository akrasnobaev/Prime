using System.Collections.Generic;
using System.Threading;
using OptimusPrime.OprimusPrimeCore;

namespace OptimusPrimeTest
{
    public class ReadService<T> : OptimusPrimeService
    {
        public readonly IList<T> TestDataCollection;
        public AutoResetEvent AutoResetEvent;
        private readonly int testDataCount;

        public ReadService(int testDataCount, string host = TestConstants.Host, int page = TestConstants.DbPage,
                           string storageKey = TestConstants.StorageKey)
            : base(host, page)
        {
            this.testDataCount = testDataCount;
            TestDataCollection = new List<T>();
            OptimusPrimeIn = new IOptimusPrimeIn[] {new OptimusPrimeIn(storageKey, this)};
            AutoResetEvent = new AutoResetEvent(false);
        }

        public override void Actuation()
        {
            for (int i = 0; i < testDataCount; i++)
                TestDataCollection.Add(OptimusPrimeIn[0].Get<T>());
            AutoResetEvent.Set();
        }
    }
}