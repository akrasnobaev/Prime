using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using Prime;

namespace OptimusPrimeTest.Prime
{
    [TestFixture]
    public abstract class LinkSourceToFilterBaseTest
    {
        private IPrimeFactory factory;
        private EventBlock<TestData> eventBlock;
        private AutoResetEvent isReadFinished;
        private List<TestData> sourseData;
        private List<TestData> resultData;
        private IReader<TestData> sourceReader;
        private const int DataCount = 6;

        protected abstract IPrimeFactory CreaFactory();

        [SetUp]
        public void SetUp()
        {
            factory = CreaFactory();
            isReadFinished = new AutoResetEvent(false);
        }

        [Test]
        public void TestGet()
        {
            var chain = new FunctionalBlock<TestData, bool>(IsEven);
            eventBlock = new EventBlock<TestData>();
            var source = factory.CreateSource(eventBlock);

            var testSource = factory.LinkSourceToFilter(source, chain);
            sourseData = TestData.CreateData(DataCount);
            sourceReader = testSource.Factory.CreateReciever(testSource).GetReader();
            resultData = sourseData.Where(z => z.Number%2 == 0).ToList();

            factory.Start();

            new Thread(() => WriteData()).Start();
            new Thread(() => ReadData()).Start();
            isReadFinished.WaitOne();

            TestData outTestData;
            Assert.IsFalse(sourceReader.TryGet(out outTestData));
        }

        private void ReadData(bool isWait = false)
        {
            var random = new Random();
            foreach (var testData in resultData)
            {
                if (isWait)
                    Thread.Sleep(random.Next(10));
                var actual = sourceReader.Get();
                testData.AssertAreEqual(actual);
            }
            isReadFinished.Set();
        }

        private void WriteData(bool isWait = false)
        {
            var random = new Random();
            foreach (var data in sourseData)
            {
                if (isWait)
                    Thread.Sleep(random.Next(10));
                eventBlock.Publish(data);
            }
        }

        private static bool IsEven(TestData input)
        {
            var result = input.Clone();
            return result.Number%2 == 0;
        }

        [TearDown]
        public void TearDown()
        {
            factory.Stop();
        }
    }
}