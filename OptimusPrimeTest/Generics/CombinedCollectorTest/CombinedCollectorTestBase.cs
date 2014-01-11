using System;
using System.Collections.Generic;
using System.Threading;
using Eurobot.Services;
using NUnit.Framework;
using OptimusPrime.Factory;
using OptimusPrime.Templates;

namespace OptimusPrimeTests.Generics
{
    public abstract class CombinedCollectorTestBase
    {
        private class Async : SourceDataCollection<int>
        {
            public List<int> Ints
            {
                get { return List0; }
            }
        }

        private class Sync : SyncronousSourceDataCollection<int>
        {
        }

        private class IntProducer
        {
            private Random rnd = new Random();
            public SourceBlock<int> Block = new SourceBlock<int>();
            private int Max;

            private void WorkThread()
            {
                for (int i = 0; i < Max; i++)
                {
                    Block.Publish(i);
                    Thread.Sleep(rnd.Next(10));
                }
            }

            public void Start(int max)
            {
                Max = max;
                new Thread(WorkThread).Start();
            }
        }

        private class RepeaterBlock : IRepeaterBlock<int, bool, int, int, Tuple<Async, Sync>>
        {
            private int Max;
            private int expectedAsync = 0;

            private int position = 0;

            public void Start(int max)
            {
                Max = max;
                position = expectedAsync = 0;
            }


            public bool Conclude()
            {
                return true;
            }


            public bool MakeIteration(Tuple<Async, Sync> sourceDatas, int oldPrivateOut, out int privateIn)
            {
                foreach (var e in sourceDatas.Item1.Ints)
                {
                    Assert.AreEqual(expectedAsync, e);
                    expectedAsync++;
                }
                if (position > 0)
                    Assert.AreEqual(oldPrivateOut, sourceDatas.Item2.Data0);

                privateIn = position;
                position++;
                return position < Max;
            }
        }

        [Test]
        public void TestCombinedCollector()
        {
            var factory = CreateFactory();
            var rnd = new Random();
            var chain = factory.CreateChain(new Func<int, int>(z =>
            {
                Thread.Sleep(rnd.Next(10));
                return z;
            }));
            var fork = chain.Fork();
            var syncCollector = factory.BindSources(fork.Source).CreateSyncCollector<Sync>();
            var producer = new IntProducer();
            var source = factory.CreateSource(producer.Block);
            var asyncCollector = factory.BindSources(source).CreateAsyncCollector<Async>();
            var combined = CombinedCollector.Create(asyncCollector, syncCollector);
            var repeater = factory.CreateRepeater(new RepeaterBlock(), combined, fork.Chain);
            factory.Start();
            producer.Start(100);
            repeater.ToFunctionalBlock().Process(100);
        }

        protected abstract IFactory CreateFactory();
    }
}
