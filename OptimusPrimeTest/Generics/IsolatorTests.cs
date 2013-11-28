using NUnit.Framework;
using OptimusPrime.Factory;
using OptimusPrime;
using OptimusPrime.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace OptimusPrimeTests.Generics
{
    [TestFixture]
    class IsolatorTests
    {
        class TestChain : IFunctionalBlock<int, int>
        {
            bool busy = false;

            public int Count = 0;

            public int Process(int input)
            {
                if (busy) Assert.Fail("Обращение к изолированному блоку осуществлено одновременно!");
                busy = true;
                Count++;
                busy = false;
                return input;
            }
        }

        [Test]
        public void IsolatorTest()
        {
            var producer = new DataProducer<int>();
            var factory=new CallFactory();
            var source = factory.CreateSource(producer.SourceBlock);
            var iso=factory.CreateChain(new TestChain()).Isolate();
            int NumPack=100;
            var out1 = source.Link(iso.CreateIsolated());
            var out2 = source.Link(iso.CreateIsolated());
            var out3 = source.Link(iso.CreateIsolated());
            var collector = factory.BindSources(out1, out2, out3).CreateSyncCollector<SyncronousSourceDataCollection<int,int,int>>();
            factory.Start();

            producer.Start(NumPack, 1, z => z);
            var watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < NumPack; i++)
                collector.Get();
            factory.Stop();
            
        }
    }
}
