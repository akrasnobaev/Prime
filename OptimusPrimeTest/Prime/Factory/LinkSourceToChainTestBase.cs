using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using OptimusPrime.Common.Exception;
using Prime;

namespace OptimusPrimeTest.Prime
{
    [TestFixture]
    public abstract class LinkSourceToChainTestBase
    {
        private IPrimeFactory factory;
        private EventBlock<TestData> eventBlock;
        private AutoResetEvent isReadFinished;
        private TestData outTestData;
        private List<TestData> testDatas;
        private IReader<TestData> sourceReader;
        private const int DataCount = 3;

        protected abstract IPrimeFactory CreateFactory();

        [SetUp]
        public void SetUp()
        {
            factory = CreateFactory();
            isReadFinished = new AutoResetEvent(false);
            eventBlock = new EventBlock<TestData>();
        }

        [Test]
        public void TestGet()
        {
            var chain = factory.CreateChain<TestData, TestData>(AddOne);
            var source = factory.CreateSource(eventBlock);

            var testSource = factory.LinkSourceToChain(source, chain);
            testDatas = TestData.CreateData(DataCount);
            sourceReader = testSource.Factory.CreateReciever(source).GetReader();

            factory.Start();

            new Thread(() => WriteData()).Start();
            new Thread(() => ReadData()).Start();
            isReadFinished.WaitOne();

            Assert.IsFalse(sourceReader.TryGet(out outTestData));
        }

        [Test, ExpectedException(typeof(ChainAlreadyUsedException))]
        public void TestTryUseAlreadyUsedChain()
        {
            var chain = factory.CreateChain<TestData, TestData>(AddOne);
            // first use
            chain.ToFunctionalBlock();

            var source = factory.CreateSource(eventBlock);
            // second use
            factory.LinkSourceToChain(source, chain);
        }

        private void ReadData(bool isWait = false)
        {
            var random = new Random();
            foreach (var testData in testDatas)
            {
                if (isWait)
                    Thread.Sleep(random.Next(10));
                var actual = sourceReader.Get();
                actual.Number--;
                testData.AssertAreEqual(actual);
            }
            isReadFinished.Set();
        }

        private void WriteData(bool isWait = false)
        {
            var random = new Random();
            foreach (var data in testDatas)
            {
                if (isWait)
                    Thread.Sleep(random.Next(10));
                eventBlock.Publish(data);
            }
        }

        private static TestData AddOne(TestData input)
        {
            var result = input.Clone();
            result.Number++;
            return result;
        }

        [TearDown]
        public void TearDown()
        {
            factory.Stop();
        }
    }
}