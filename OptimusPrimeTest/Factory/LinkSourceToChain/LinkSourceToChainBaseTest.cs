using System;
using System.Collections.Generic;
using System.Threading;
using Eurobot.Services;
using NUnit.Framework;
using OptimusPrime.Factory;
using OptimusPrime.Templates;
using OptimusPrimeTest;

namespace OptimusPrimeTests.Factory.LinkSourceToChain
{
    [TestFixture]
    public abstract class LinkSourceToChainBaseTest
    {
        private IFactory factory;
        private SourceBlock<TestData> sourceBlock;
        private AutoResetEvent isReadFinished;
        private TestData outTestData;
        private List<TestData> testDatas;
        private ISourceReader<TestData> sourceReader;

        protected abstract IFactory CreaFactory();
        
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
            testDatas = TestData.CreateData(100);
            sourceReader = testSource.CreateReader();

            factory.Start();

            new Thread(() => WriteData(true)).Start();
            new Thread(() => ReadData(true)).Start();
            isReadFinished.WaitOne();

            Assert.IsFalse(sourceReader.TryGet(out outTestData));
            Assert.AreEqual(source.Name, chain.InputName);
            Assert.AreEqual(testSource.Name, chain.OutputName);
        }

        private void ReadData(bool isWait)
        {
            var random = new Random();
            foreach (var testData in testDatas)
            {
                if(isWait)
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
                if(isWait)
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