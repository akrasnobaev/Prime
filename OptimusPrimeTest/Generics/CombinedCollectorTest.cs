using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OptimusPrime.Factory;
using OptimusPrime.Templates;

namespace OptimusPrimeTests.Generics
{
    public class CombinedCollectorTest
    {
        class Async : SourceDataCollection<int> { public List<int> Ints { get { return List0; } } }

        class Sync : SyncronousSourceDataCollection<int> { }

        class RepeaterBlock : IRepeaterBlock<int,bool,int,int,Tuple<Async,Sync>>
        {
            int Max;
            int expectedAsync = 0;

            int position = 0;

            public void Start(int max)
            {
                Max = max;
                position = expectedAsync = 0;
            }


            public bool Conclude()
            {
                throw new NotImplementedException();
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
                position++;
                privateIn = position;
                return position < Max;
            }
        }

        void Test(IFactory factory)
        {
            var chain = factory.CreateChain(new Func<int, int>(z => z));
            var fork1 = chain.Fork();
            var fork2 = fork1.Chain.Fork();
            var asyncCollector = factory.BindSources(fork1.Source).CreateAsyncCollector<Async>();
            var syncCollector = factory.BindSources(fork2.Source).CreateSyncCollector<Sync>();
            var combined = CombinedCollector.Create(asyncCollector, syncCollector);
            var repeater = factory.CreateRepeater(new RepeaterBlock(), combined, fork2.Chain);
            factory.Start();
            repeater.ToFunctionalBlock().Process(10000);
        }

        [Test]
        public void CombinedCollectorLiberty()
        {
            Test(new CallFactory());
        }

        [Test]
        public void CombinedCollectorOptimus()
        {
            Test(new OptimusPrimeFactory());
        }
    }
}
