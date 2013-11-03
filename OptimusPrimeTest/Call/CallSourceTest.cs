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
    public class CallSourceTest
    {
        [SetUp]
        public void Setup()
        {
            isReadEnd = new AutoResetEvent(false);
            factory = new CallFactory();
            sourceBlock = new SourceBlock<TestData>();
            callSource = factory.CreateSource(sourceBlock);
            firstCallReader = callSource.CreateReader();
            secondCallReader = callSource.CreateReader();
            factory.Start();
        }

        [TearDown]
        public void TearDown()
        {
            factory.Stop();
        }

        private AutoResetEvent isReadEnd;
        private CallFactory factory;
        private SourceBlock<TestData> sourceBlock;
        private ISourceReader<TestData> firstCallReader;
        private TestData outTestData;
        private ISource<TestData> callSource;
        private ISourceReader<TestData> secondCallReader;

        private void WriteData(SourceBlock<TestData> source, IEnumerable<TestData> datas, bool isWait = false)
        {
            var random = new Random();
            foreach (TestData data in datas)
            {
                if (isWait)
                    Thread.Sleep(random.Next(10));
                source.Publish(data);
            }
        }

        private void ReadData(ISourceReader<TestData> sourceReader, IEnumerable<TestData> datas, bool isWait = false)
        {
            var random = new Random();
            foreach (TestData data in datas)
            {
                if (isWait)
                    Thread.Sleep(random.Next(10));
                data.AssertAreEqual(sourceReader.Get());
            }
            isReadEnd.Set();
        }

        private void Get()
        {
            List<TestData> testDatas = TestData.CreateData(1000);

            new Thread(() => WriteData(sourceBlock, testDatas, true)).Start();
            new Thread(() => ReadData(firstCallReader, testDatas, true)).Start();
            isReadEnd.WaitOne();

            Assert.IsFalse(firstCallReader.TryGet(out outTestData));
        }

        private void GetRange()
        {
            List<TestData> testDatas = TestData.CreateData(1000);

            WriteData(sourceBlock, testDatas);
            TestData[] actual = firstCallReader.GetCollection().ToArray();
            Assert.AreEqual(testDatas.Count, actual.Length);

            for (int i = 0; i < testDatas.Count; i++)
                testDatas[i].AssertAreEqual(actual[i]);

            Assert.IsFalse(firstCallReader.TryGet(out outTestData));
        }

        private void TryGet()
        {
            List<TestData> testDatas = TestData.CreateData(1000);

            WriteData(sourceBlock, testDatas);
            foreach (TestData testData in testDatas)
            {
                Assert.IsTrue(firstCallReader.TryGet(out outTestData));
                testData.AssertAreEqual(outTestData);
            }

            Assert.IsFalse(firstCallReader.TryGet(out outTestData));
        }

        [Test]
        public void TestGetUsingSeveralReaders()
        {
            List<TestData> testDatas = TestData.CreateData(1000);

            new Thread(() => WriteData(sourceBlock, testDatas, true)).Start();
            new Thread(() => ReadData(firstCallReader, testDatas, true)).Start();
            isReadEnd.WaitOne();

            ReadData(secondCallReader, testDatas);

            Assert.IsFalse(firstCallReader.TryGet(out outTestData));
            Assert.IsFalse(secondCallReader.TryGet(out outTestData));
        }

        [Test]
        public void TestGetWhenCallReaderCreateAfterWrite()
        {
            List<TestData> testDatas = TestData.CreateData(1000);
            WriteData(sourceBlock, testDatas);

            var thirdReader = callSource.CreateReader();
            ReadData(thirdReader, testDatas);

            Assert.IsFalse(thirdReader.TryGet(out outTestData));
        }

        [Test]
        public void TestGet()
        {
            Get();
        }

        [Test]
        public void TestGetAfterGetRange()
        {
            GetRange();
            Get();
        }

        [Test]
        public void TestGetAfterTryGet()
        {
            TryGet();
            Get();
        }

        [Test]
        public void TestGetRange()
        {
            GetRange();
        }

        [Test]
        public void TestGetRangeAfterGet()
        {
            Get();
            GetRange();
        }

        [Test]
        public void TestGetRangeAfterTryGet()
        {
            TryGet();
            GetRange();
        }

        [Test]
        public void TestTryGet()
        {
            TryGet();
        }

        [Test]
        public void TestTryGetAfterGet()
        {
            Get();
            TryGet();
        }

        [Test]
        public void TestTryGetAfterGetRange()
        {
            GetRange();
            TryGet();
        }
    }
}