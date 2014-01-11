using System.Collections.Generic;
using System.Threading;
using Eurobot.Services;
using NUnit.Framework;
using OptimusPrime.Factory;
using OptimusPrime.Templates;
using OptimusPrimeTest;

namespace OptimusPrimeTests.Generics
{
    [TestFixture]
    public abstract class ListenerTestBase
    {
        private const int DataCount = 3;

        private SourceBlock<TestData> sourceBlock;
        private IFactory factory;
        private ISource<TestData> source;
        private List<TestData> testData;

        protected abstract IFactory CreateFactory();

        [Test]
        public void TestListener()
        {
            testData = TestData.CreateData(DataCount);

            sourceBlock = new SourceBlock<TestData>();
            factory = CreateFactory();
            source = factory.CreateSource(sourceBlock);

            var readCount = 0;
            source.Listen(data => testData[readCount++].AssertAreEqual(data));

            factory.Start();
            foreach (var data in testData)
                sourceBlock.Publish(data);

            /// Ожидание окончания работы слушателя.
            /// нет общего механизма уведомления об окончании работы, и делать пока нет необходимости.
            Thread.Sleep(100);

            factory.Stop();

            Assert.AreEqual(DataCount, readCount);
        }
    }
}
