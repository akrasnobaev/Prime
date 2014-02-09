using System;
using NUnit.Framework;
using Prime;

namespace OptimusPrimeTest.Prime
{
    [TestFixture]
    public abstract class RepeaterTestBase
    {
        private const int SourceRepetition = 10;

        private class Emulator : IFunctionalBlock<int, int>
        {
            public readonly SourceBlock<int> Source = new SourceBlock<int>();

            public int Process(int input)
            {
                for (int i = 0; i < SourceRepetition; i++)
                    Source.Publish(input);
                return input;
            }
        }

        private class Repeater : IRepeaterBlock<int, int, int, int, Tuple<int[], int[]>>
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
        public void TestRepeater()
        {
            var factory = CreateFactory();
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

        protected abstract IPrimeFactory CreateFactory();
    }
}