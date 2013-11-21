using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OptimusPrime.Factory;
using OptimusPrime.Templates;
using Eurobot.Services;
using System.Threading;
using OptimusPrime;

namespace OptimusPrimeTests.Generics
{
    public class CombinedCollectorTest
    {
        

        class Async : SourceDataCollection<int> { public List<int> Ints { get { return List0; } } }

        class Sync : SyncronousSourceDataCollection<int> { }

        class IntProducer
        {
            Random rnd=new Random();
            public SourceBlock<int> Block=new SourceBlock<int>();
            int Max;
            void WorkThread()
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

        void Test(IFactory factory)
        {
            var rnd=new Random();
            var chain = factory.CreateChain(new Func<int, int>(z => { Thread.Sleep(rnd.Next(10)); return z; }));
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
