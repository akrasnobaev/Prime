using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using BookSleeve;
using NUnit.Framework;
using OptimusPrimeTest.Prime;
using Prime.Optimus;

namespace OptimusPrimeTest.OptimusPrime
{
    [TestFixture]
    public class OptimusInTest
    {
        private const int DataCount = 3;

        [SetUp]
        public void Setup()
        {
            testData = TestData.CreateData(DataCount);
            var connection = new RedisConnection("localhost", allowAdmin: true);

            Task openTask = connection.Open();
            connection.Wait(openTask);

            Task flushDbTask = connection.Server.FlushAll();
            connection.Wait(flushDbTask);

            stabService = new OptimusStabService(dbPage: 2);
            _optimusIn = new OptimusIn(storageKey, stabService);
            isGetFinished = new AutoResetEvent(false);
        }

        private const string storageKey = "testStotageKey";
        private OptimusIn _optimusIn;
        private IOptimusService stabService;
        private List<TestData> testData;
        private AutoResetEvent isGetFinished;
        private TestData outTestData;

        private void WriteData(string setStorageKey, IEnumerable<TestData> testDatas, bool isWait = false)
        {
            var optimusPrimeOut = new OptimusOut(setStorageKey, stabService, new Stopwatch(), true);
            var random = new Random();
            foreach (TestData data in testDatas)
            {
                if (isWait)
                    Thread.Sleep(random.Next(5));
                optimusPrimeOut.Set(data);
            }
        }

        private void ReadData(OptimusIn input, IEnumerable<TestData> expectedData, bool isWait = false)
        {
            var random = new Random();
            foreach (TestData data in expectedData)
            {
                if (isWait)
                    Thread.Sleep(random.Next(5));
                data.AssertAreEqual(input.Get<TestData>());
            }
            isGetFinished.Set();
        }

        [Test]
        public void TestChangeNameSetDataAfterChangeName()
        {
            const string otherKey = "otherStorageKye";
            List<TestData> otherData = TestData.CreateData(DataCount);

            var otherIn = new OptimusIn(otherKey, stabService);
            new Thread(() => WriteData(otherKey, otherData)).Start();
            new Thread(() => ReadData(otherIn, otherData)).Start();
            isGetFinished.WaitOne();

            otherIn.ChangeName(storageKey);
            new Thread(() => WriteData(storageKey, testData)).Start();
            new Thread(() => ReadData(otherIn, testData)).Start();
            isGetFinished.WaitOne();

            Assert.IsFalse(otherIn.TryGet(out outTestData));
        }

        [Test]
        public void TestChangeNameSetDataBeforeChangeName()
        {
            const string otherKey = "otherStorageKye";
            List<TestData> otherData = TestData.CreateData(DataCount);

            WriteData(storageKey, testData);

            var otherIn = new OptimusIn(otherKey, stabService);
            new Thread(() => WriteData(otherKey, otherData)).Start();
            new Thread(() => ReadData(otherIn, otherData)).Start();
            isGetFinished.WaitOne();

            otherIn.ChangeName(storageKey);
            ReadData(otherIn, testData);

            Assert.IsFalse(otherIn.TryGet(out outTestData));
        }

        [Test]
        public void TestGet()
        {
            new Thread(() => WriteData(storageKey, testData)).Start();
            new Thread(() => ReadData(_optimusIn, testData)).Start();
            isGetFinished.WaitOne();

            Assert.IsFalse(_optimusIn.TryGet(out outTestData));
        }

        [Test]
        public void TestGetAfterTryGet()
        {
            WriteData(storageKey, testData);
            foreach (TestData data in testData)
            {
                Assert.IsTrue(_optimusIn.TryGet(out outTestData));
                data.AssertAreEqual(outTestData);
            }

            var otherData = TestData.CreateData(DataCount);
            new Thread(() => WriteData(storageKey, otherData)).Start();
            new Thread(() => ReadData(_optimusIn, otherData)).Start();
            isGetFinished.WaitOne();

            Assert.IsFalse(_optimusIn.TryGet(out outTestData));
        }
        
        [Test]
        public void TestGetAfterGetRange()
        {
            WriteData(storageKey, testData);
            TestData[] actual = _optimusIn.GetRange<TestData>();

            for (int i = 0; i < testData.Count; i++)
                testData[i].AssertAreEqual(actual[i]);

            var otherData = TestData.CreateData(DataCount);
            new Thread(() => WriteData(storageKey, otherData)).Start();
            new Thread(() => ReadData(_optimusIn, otherData)).Start();
            isGetFinished.WaitOne();

            Assert.IsFalse(_optimusIn.TryGet(out outTestData));
        }

        [Test]
        public void TestGetRange()
        {
            WriteData(storageKey, testData);
            TestData[] actual = _optimusIn.GetRange<TestData>();

            for (int i = 0; i < testData.Count; i++)
                testData[i].AssertAreEqual(actual[i]);

            Assert.IsFalse(_optimusIn.TryGet(out outTestData));
        }

        [Test]
        public void TestGetRangeAfterGet()
        {
            var otherData = TestData.CreateData(DataCount);
            new Thread(() => WriteData(storageKey, otherData)).Start();
            new Thread(() => ReadData(_optimusIn, otherData)).Start();
            isGetFinished.WaitOne();

            WriteData(storageKey, testData);
            TestData[] actual = _optimusIn.GetRange<TestData>();

            for (int i = 0; i < testData.Count; i++)
                testData[i].AssertAreEqual(actual[i]);

            Assert.IsFalse(_optimusIn.TryGet(out outTestData));
        }

        [Test]
        public void TestGetRangeAfterTryGet()
        {
            var otherData = TestData.CreateData(DataCount);
            WriteData(storageKey, otherData);
            foreach (TestData data in otherData)
            {
                Assert.IsTrue(_optimusIn.TryGet(out outTestData));
                data.AssertAreEqual(outTestData);
            }

            WriteData(storageKey, testData);
            TestData[] actual = _optimusIn.GetRange<TestData>();

            for (int i = 0; i < testData.Count; i++)
                testData[i].AssertAreEqual(actual[i]);

            Assert.IsFalse(_optimusIn.TryGet(out outTestData));
        }

        [Test]
        public void TestTryGet()
        {
            WriteData(storageKey, testData);
            foreach (TestData data in testData)
            {
                Assert.IsTrue(_optimusIn.TryGet(out outTestData));
                data.AssertAreEqual(outTestData);
            }

            Assert.IsFalse(_optimusIn.TryGet(out outTestData));
        }

        [Test]
        public void TestTryGetAfterGet()
        {
            var otherData = TestData.CreateData(DataCount);
            new Thread(() => WriteData(storageKey, otherData)).Start();
            new Thread(() => ReadData(_optimusIn, otherData)).Start();
            isGetFinished.WaitOne();

            WriteData(storageKey, testData);
            foreach (TestData data in testData)
            {
                Assert.IsTrue(_optimusIn.TryGet(out outTestData));
                data.AssertAreEqual(outTestData);
            }

            Assert.IsFalse(_optimusIn.TryGet(out outTestData));
        }

        [Test]
        public void TestTryGetAfterGetGange()
        {
            var otherData = TestData.CreateData(DataCount);
            WriteData(storageKey, otherData);
            TestData[] actual = _optimusIn.GetRange<TestData>();

            for (int i = 0; i < testData.Count; i++)
                otherData[i].AssertAreEqual(actual[i]);

            WriteData(storageKey, testData);
            foreach (TestData data in testData)
            {
                Assert.IsTrue(_optimusIn.TryGet(out outTestData));
                data.AssertAreEqual(outTestData);
            }

            Assert.IsFalse(_optimusIn.TryGet(out outTestData));
        }
    }
}