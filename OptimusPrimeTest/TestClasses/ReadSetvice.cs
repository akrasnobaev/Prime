using System.Collections.Generic;
using System.Threading;
using OptimusPrime.OprimusPrimeCore;

namespace OptimusPrimeTest
{
    public class ReadService<T> : OptimusPrimeService
    {
        public IList<T> TestDataCollection;
        public AutoResetEvent AutoResetEvent;
        private readonly int _testDataCount;

        public ReadService(int testDataCount, string storageKey = TestConstants.StorageKey)
            : base(TestConstants.Host, TestConstants.DbPage)
        {
            _testDataCount = testDataCount;

            TestDataCollection = new List<T>();
            OptimusPrimeIn = new IOptimusPrimeIn[] { new OptimusPrimeIn(storageKey, this) };
            AutoResetEvent = new AutoResetEvent(false);
        }

        public override void Initialize()
        {
        }

        public override void DoWork()
        {
            for (int i = 0; i < _testDataCount; i++)
                TestDataCollection.Add(OptimusPrimeIn[0].Get<T>());
            AutoResetEvent.Set();
        }
    }
}