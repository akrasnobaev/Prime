using System.Threading;
using Eurobot.Services;
using NUnit.Framework;
using OptimusPrime.Factory;
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
            var collector =
                factory.BindSources(out1, out2, out3)
                    .CreateSyncCollector<SyncronousSourceDataCollection<TestData, TestData, TestData>>();
            var testData = TestData.CreateData(DataCount);

            factory.Start();

            /// FIXME Issue #135
            collector.SkipFirstGet = false;

            foreach (var data in testData)
            {
                sourceBlock.Publish(data);
                var actual = collector.Get();

                actual.Data0.AssertAreEqual(data);
                actual.Data1.AssertAreEqual(data);
                actual.Data2.AssertAreEqual(data);
            }

            factory.Stop();
        }
    }
}
