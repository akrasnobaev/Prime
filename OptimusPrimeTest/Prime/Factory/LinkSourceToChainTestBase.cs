using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Prime;

namespace OptimusPrimeTest.Prime
{
    [TestFixture]
    public abstract class LinkSourceToChainTestBase
    {
        private IPrimeFactory factory;
        private SourceBlock<TestData> sourceBlock;
        private AutoResetEvent isReadFinished;
        private TestData outTestData;
        private List<TestData> testDatas;
        private ISourceReader<TestData> sourceReader;
        private const int DataCount = 3;

        protected abstract IPrimeFactory CreaFactory();

        [SetUp]
        public void SetUp()
        {
            factory = CreaFactory();
            isReadFinished = new AutoResetEvent(false);
        }

        [Test]
        public void TestGet()
        {
            var chain = factory.CreateChain<TestData, TestData>(AddOne);
            sourceBlock = new SourceBlock<TestData>();
            var source = factory.CreateSource(sourceBlock);

            var testSource = factory.LinkSourceToChain(source, chain);
            testDatas = TestData.CreateData(DataCount);
            sourceReader = testSource.CreateReader();

            factory.Start();

            new Thread(() => WriteData()).Start();
            new Thread(() => ReadData()).Start();
            isReadFinished.WaitOne();

            Assert.IsFalse(sourceReader.TryGet(out outTestData));
            Assert.AreEqual(source.Name, chain.InputName);
            Assert.AreEqual(testSource.Name, chain.OutputName);
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
                sourceBlock.Publish(data);
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