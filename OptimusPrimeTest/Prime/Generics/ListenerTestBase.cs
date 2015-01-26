using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Prime;

namespace OptimusPrimeTest.Prime
{
    [TestFixture]
    public abstract class ListenerTestBase
    {
        private const int DataCount = 3;

        private EventBlock<TestData> eventBlock;
        private IPrimeFactory factory;
        private ISource<TestData> source;
        private List<TestData> testData;

        protected abstract IPrimeFactory CreateFactory();

        [Test]
        public void TestListener()
        {
            testData = TestData.CreateData(DataCount);

            eventBlock = new EventBlock<TestData>();
            factory = CreateFactory();
            source = factory.CreateSource(eventBlock);

            var readCount = 0;
            source.Listen(data => testData[readCount++].AssertAreEqual(data));

            factory.Start();
            foreach (var data in testData)
                eventBlock.Publish(data);

            /// Ожидание окончания работы слушателя.
            /// нет общего механизма уведомления об окончании работы, и делать пока нет необходимости.
            Thread.Sleep(100);

            factory.Stop();

            Assert.AreEqual(DataCount, readCount);
        }
    }
}
