using System;
using System.Threading;
using NUnit.Framework;
using Prime;

namespace OptimusPrimeTest.Prime
{
    public abstract class CombinedCollectorTestBase
    {
        class IntProducer
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

        class RepeaterBlock : IRepeaterBlock<int,bool,int,int,Tuple<int[], int>>
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


            public bool MakeIteration(Tuple<int[], int> sourceDatas, int oldPrivateOut, out int privateIn)
            {
                foreach (var e in sourceDatas.Item1)
                {
                    Assert.AreEqual(expectedAsync, e);
                    expectedAsync++;
                }
                if (position > 0)
                    Assert.AreEqual(oldPrivateOut, sourceDatas.Item2);

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
            var producer = new IntProducer();
            var source = factory.CreateSource(producer.Block);

            var combined = factory.Union(
                source.CreateAsyncCollector().CreateRepeaterAdapter(),
                source.CreateSyncCollector().CreateRepeaterAdapter());

            var repeater = factory.CreateRepeater(new RepeaterBlock(), combined, fork.Chain);
            factory.Start();
            producer.Start(100);
            repeater.ToFunctionalBlock().Process(100);
        }

        protected abstract IPrimeFactory CreateFactory();
    }
}
