using NUnit.Framework;
using OptimusPrime.Templates;
using Eurobot.Services;
using OptimusPrime.Factory;
using System.Linq;
using System.Collections.Generic;

namespace OptimusPrimeTest
{
    [TestFixture]
    public class RepeaterTest
    {
        static int SourceRepetition = 10; 
        public class Int2Collector : SourceDataCollection<int,int>
        {
            public List<int> List_0 { get { return List0; } }
            public List<int> List_1 { get { return List0; } }
        }

        public class Emulator : IFunctionalBlock<int, int>
        {
           public SourceBlock<int> Source = new SourceBlock<int>();

            public int Process(int input)
            {
                for (int i=0;i<SourceRepetition;i++)
                    Source.Publish(input);
                return input;
            }
        }

        public class Repeater : IRepeaterBlock<int, int, int, int, Int2Collector>
        {
            int Count;
            int current;

            public void Start(int publicIn)
            {
                Count = publicIn;
                current = 0;
            }

            public bool MakeIteration(Int2Collector sourceDatas, int oldPrivateOut, out int privateIn)
            {
                Assert.AreEqual(current, oldPrivateOut);
                if (current>0)
                    Assert.AreEqual(SourceRepetition, sourceDatas.List_0.Count);
                foreach (var e in sourceDatas.List_0)
                    Assert.AreEqual(current, e);
                current++;
                privateIn = current;
                return current < Count;
            }

            public int Conclude()
            {
                return current;
            }
        }

        [Test]
        public void CallSourceReaderWithSingleData()
        {
            MakeCallTest(new CallFactory());
        }

        private static void MakeCallTest(IFactory factory)
        {
            var block = new SourceBlock<int>();
            var source = factory.CreateSource(block);
            var reader = source.CreateReader();

            factory.Start();
            block.Publish(10);
            Assert.AreEqual(10, reader.Get());
            factory.Stop();
        }

        [Test]
        public void CallSourceReaderWithCollection()
        {
            var factory = new CallFactory();
            var block = new SourceBlock<int>();
            var source = factory.CreateSource(block);
            var reader = source.CreateReader();
            for (int i=0;i<10;i++)
                block.Publish(10);
            var col = reader.GetCollection().ToList();
            Assert.AreEqual(10, col.Count);
        }

        void MakeRepeaterTest(IFactory factory)
        {

            var emulator = new Emulator();

            var smallChain = factory.CreateChain(emulator);
            var source = factory.CreateSource(emulator.Source);
            var collector = factory.BindSources(source, source).CreateCollector<Int2Collector>();
            var rep = factory.CreateRepeater(new Repeater(), collector, smallChain);

            factory.Start();
            Assert.AreEqual(10, rep.ToFunctionalBlock().Process(10));
            factory.Stop();
        }

        [Test]
        public void RepeaterTestCall()
        {
            MakeRepeaterTest(new CallFactory());
        }

        [Test]
        public void RepeaterTestOP()
        {
            MakeRepeaterTest(new OptimusPrimeFactory());
        }
    }
}
