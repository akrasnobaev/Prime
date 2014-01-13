using NUnit.Framework;
using Prime;
using System.Linq;
using System;

namespace OptimusPrimeTest
{
    [TestFixture]
    public class RepeaterTest
    {
        private static int SourceRepetition = 10;


        public class Emulator : IFunctionalBlock<int, int>
        {
            public SourceBlock<int> Source = new SourceBlock<int>();

            public int Process(int input)
            {
                for (int i = 0; i < SourceRepetition; i++)
                    Source.Publish(input);
                return input;
            }
        }

        public class Repeater : IRepeaterBlock<int, int, int, int, Tuple<int[], int[]>>
        {
            private int Count;
            private int current;

            public void Start(int publicIn)
            {
                Count = publicIn;
                current = 0;
            }

            public bool MakeIteration(Tuple<int[], int[]> sourceDatas, int oldPrivateOut, out int privateIn)
            {
                Assert.AreEqual(current, oldPrivateOut);
                if (current > 0)
                    Assert.AreEqual(SourceRepetition, sourceDatas.Item1.Length);
                foreach (var e in sourceDatas.Item1)
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
            MakeCallTest(new LibertyFactory());
        }

        private static void MakeCallTest(IPrimeFactory factory)
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
            var factory = new LibertyFactory();
            var block = new SourceBlock<int>();
            var source = factory.CreateSource(block);
            var reader = source.CreateReader();
            for (int i = 0; i < 10; i++)
                block.Publish(10);
            var col = reader.GetCollection().ToList();
            Assert.AreEqual(10, col.Count);
        }

        private void MakeRepeaterTest(IPrimeFactory factory)
        {
            var emulator = new Emulator();

            var smallChain = factory.CreateChain(emulator);
            var source = factory.CreateSource(emulator.Source);

            var collector = factory.Union(
                source.CreateAsyncCollector().CreateRepeaterAdapter(),
                source.CreateAsyncCollector().CreateRepeaterAdapter());

            var rep = factory.CreateRepeater(new Repeater(), collector, smallChain);

            factory.Start();
            Assert.AreEqual(10, rep.ToFunctionalBlock().Process(10));
            factory.Stop();
        }

        [Test]
        public void RepeaterTestCall()
        {
            MakeRepeaterTest(new LibertyFactory());
        }

        [Test]
        public void RepeaterTestOP()
        {
            MakeRepeaterTest(new PrimeFactory());
        }
    }
}
