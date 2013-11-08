using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Eurobot.Services;
using NUnit.Framework;
using OptimusPrime.Factory;
using OptimusPrime.Templates;

namespace OptimusPrimeTest.Call
{
    [TestFixture]
    public class CallFilteringTest
    {
        private CallFactory factory;
        private IChain<TestData, bool> chain;
        private SourceBlock<TestData> sourceBlock;
        private ISource<TestData> source;
        private AutoResetEvent isReadFinished;
        private TestData outTestData;

        [SetUp]
        public void SetUp()
        {
            factory = new CallFactory();
            isReadFinished = new AutoResetEvent(false);
        }

        [Test]
        public void TestGet()
        {
            chain = factory.CreateChain<TestData, bool>(AddOne);
            sourceBlock = new SourceBlock<TestData>();
            source = factory.CreateSource(sourceBlock);

            var testSource = factory.LinkSourceToFilter(source, chain);
            var testDatas = TestData.CreateData(100);
            var sourceReader = testSource.CreateReader();

            factory.Start();

            new Thread(() => WriteData(testDatas, true)).Start();
            new Thread(() => ReadData(testDatas.Where(z=>z.Number>10).ToList(), sourceReader, true)).Start();
            isReadFinished.WaitOne();

            Assert.IsFalse(sourceReader.TryGet(out outTestData));
            Assert.AreEqual(source.Name, chain.InputName);
            Assert.AreEqual(testSource.Name, chain.OutputName);
        }

        private void ReadData(IEnumerable<TestData> testDatas, ISourceReader<TestData> reader, bool isWait)
        {
            var random = new Random();
            foreach (var testData in testDatas)
            {
                if(isWait)
                    Thread.Sleep(random.Next(10));
                var actual = reader.Get();
                testData.AssertAreEqual(actual);
            }
            isReadFinished.Set();
        }

        private void WriteData(IEnumerable<TestData> datas, bool isWait = false)
        {
            var random = new Random();
            foreach (var data in datas)
            {
                if(isWait)
                    Thread.Sleep(random.Next(10));
                sourceBlock.Publish(data);
            }
        }

        private static bool AddOne(TestData input)
        {
            var result = input.Clone();
            return result.Number > 10;
        }

        [TearDown]
        public void TearDown()
        {
            factory.Stop();
        }
    }
}