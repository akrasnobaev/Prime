﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using Prime;

namespace OptimusPrimeTest.Prime
{
    [TestFixture]
    public abstract class SourceReaderTestBase
    {
        private const int DataCount = 3;
        protected abstract IPrimeFactory CreateFactory();

        [SetUp]
        public void Setup()
        {
            isReadEnd = new AutoResetEvent(false);
            factory = CreateFactory();
            eventBlock = new EventBlock<TestData>();
            callSource = factory.CreateSource(eventBlock);
            firstCallReader = callSource.Factory.CreateReciever(callSource).GetReader();
            secondCallReader = callSource.Factory.CreateReciever(callSource).GetReader();
            factory.Start();
        }

        [TearDown]
        public void TearDown()
        {
            factory.Stop();
        }

        private AutoResetEvent isReadEnd;
        private IPrimeFactory factory;
        private EventBlock<TestData> eventBlock;
        private IReader<TestData> firstCallReader;
        private TestData outTestData;
        private ISource<TestData> callSource;
        private IReader<TestData> secondCallReader;

        private void WriteData(EventBlock<TestData> @event, IEnumerable<TestData> datas, bool isWait = false)
        {
            var random = new Random();
            foreach (TestData data in datas)
            {
                if (isWait)
                    Thread.Sleep(random.Next(10));
                @event.Publish(data);
            }
        }

        private void ReadData(IReader<TestData> sourceReader, IEnumerable<TestData> datas, bool isWait = false)
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
            List<TestData> testDatas = TestData.CreateData(DataCount);

            new Thread(() => WriteData(eventBlock, testDatas, true)).Start();
            new Thread(() => ReadData(firstCallReader, testDatas, true)).Start();
            isReadEnd.WaitOne();

            Assert.IsFalse(firstCallReader.TryGet(out outTestData));
        }

        private void GetRange()
        {
            List<TestData> testDatas = TestData.CreateData(DataCount);

            WriteData(eventBlock, testDatas);
            TestData[] actual = firstCallReader.GetCollection().ToArray();
            Assert.AreEqual(testDatas.Count, actual.Length);

            for (int i = 0; i < testDatas.Count; i++)
                testDatas[i].AssertAreEqual(actual[i]);

            Assert.IsFalse(firstCallReader.TryGet(out outTestData));
        }

        private void TryGet()
        {
            List<TestData> testDatas = TestData.CreateData(DataCount);

            WriteData(eventBlock, testDatas);
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
            List<TestData> testDatas = TestData.CreateData(DataCount);

            new Thread(() => WriteData(eventBlock, testDatas, true)).Start();
            new Thread(() => ReadData(firstCallReader, testDatas, true)).Start();
            isReadEnd.WaitOne();

            ReadData(secondCallReader, testDatas);

            Assert.IsFalse(firstCallReader.TryGet(out outTestData));
            Assert.IsFalse(secondCallReader.TryGet(out outTestData));
        }

        [Test]
        public void TestGetWhenCallReaderCreateAfterWrite()
        {
            List<TestData> testDatas = TestData.CreateData(DataCount);
            WriteData(eventBlock, testDatas);

            var thirdReader = callSource.Factory.CreateReciever(callSource).GetReader();
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