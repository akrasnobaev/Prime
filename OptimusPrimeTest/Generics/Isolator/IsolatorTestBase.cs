using System.Threading;
using Eurobot.Services;
using NUnit.Framework;
using OptimusPrime.Factory;
using OptimusPrime.Generics;
using OptimusPrime.Templates;
using OptimusPrimeTest;

namespace OptimusPrimeTests.Generics
{
    [TestFixture]
    public abstract class IsolatorTestBase
    {
        private const int DataCount = 3;

        private class TestChain : IFunctionalBlock<TestData, TestData>
        {
            private bool busy = false;

            public TestData Process(TestData input)
            {
                if (busy) Assert.Fail("Обращение к изолированному блоку осуществлено одновременно!");

                busy = true;
                Thread.Sleep(10);
                busy = false;

                return input;
            }
        }

        protected abstract IFactory CreateFactory();

        [Test]
        public void IsolatorTest()
        {
            var factory = CreateFactory();
            var sourceBlock = new SourceBlock<TestData>();
            var source = factory.CreateSource(sourceBlock);
            var iso = factory.CreateChain(new TestChain()).Isolate();
            var out1 = source.Link(iso.CreateIsolated());
            var out2 = source.Link(iso.CreateIsolated());
            var out3 = source.Link(iso.CreateIsolated());
            var collector = factory.Union(
                out1.CreateSyncCollector().CollectorChain,
                out2.CreateSyncCollector().CollectorChain,
                out3.CreateSyncCollector().CollectorChain
                ).ToFunctionalBlock();
            var testData = TestData.CreateData(DataCount);

            factory.Start();

            foreach (var data in testData)
            {
                sourceBlock.Publish(data);
                var actual = collector.Process(Token.Empty);

                actual.Item1.AssertAreEqual(data);
                actual.Item2.AssertAreEqual(data);
                actual.Item3.AssertAreEqual(data);
            }

            factory.Stop();
        }
    }
}
