using System.Collections.Generic;
using System.Threading;
using Prime.Optimus;

namespace OptimusPrimeTest
{
    public class ReadService<T> : OptimusService
    {
        public IList<T> TestDataCollection;
        public AutoResetEvent AutoResetEvent;
        private readonly int _testDataCount;

        public ReadService(int testDataCount, string storageKey = TestConstants.StorageKey)
            : base(TestConstants.Host, TestConstants.DbPage)
        {
            _testDataCount = testDataCount;

            TestDataCollection = new List<T>();
            OptimusIn = new IOptimusIn[] { new OptimusIn(storageKey, this) };
            AutoResetEvent = new AutoResetEvent(false);
        }

        public override void Initialize()
        {
        }

        public override void DoWork()
        {
            for (int i = 0; i < _testDataCount; i++)
                TestDataCollection.Add(OptimusIn[0].Get<T>());
            AutoResetEvent.Set();
        }
    }
}