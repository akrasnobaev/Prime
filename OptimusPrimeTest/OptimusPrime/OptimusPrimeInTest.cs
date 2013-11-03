using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookSleeve;
using NUnit.Framework;
using OptimusPrime.OprimusPrimeCore;

namespace OptimusPrimeTest
{
    [TestFixture]
    public class OptimusPrimeInTest
    {
        [SetUp]
        public void Setup()
        {
            testData = TestData.CreateData(100);
            var connection = new RedisConnection("localhost", allowAdmin: true);

            Task openTask = connection.Open();
            connection.Wait(openTask);

            Task flushDbTask = connection.Server.FlushAll();
            connection.Wait(flushDbTask);

            stabService = new OptimusPrimeStabService(dbPage: 2);
            optimusPrimeIn = new OptimusPrimeIn(storageKey, stabService);
            isGetFinished = new AutoResetEvent(false);
        }

        private const string storageKey = "testStotageKey";
        private OptimusPrimeIn optimusPrimeIn;
        private IOptimusPrimeService stabService;
        private List<TestData> testData;
        private AutoResetEvent isGetFinished;
        private TestData outTestData;

        private void WriteData(string setStorageKey, IEnumerable<TestData> testDatas, bool isWait = false)
        {
            var optimusPrimeOut = new OptimusPrimeOut(setStorageKey, stabService);
            var random = new Random();
            foreach (TestData data in testDatas)
            {
                if (isWait)
                    Thread.Sleep(random.Next(5));
                optimusPrimeOut.Set(data);
            }
        }

        private void ReadData(OptimusPrimeIn input, IEnumerable<TestData> expectedData, bool isWait = false)
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
            List<TestData> otherData = TestData.CreateData(100);

            var otherIn = new OptimusPrimeIn(otherKey, stabService);
            new Thread(() => WriteData(otherKey, otherData, true)).Start();
            new Thread(() => ReadData(otherIn, otherData, true)).Start();
            isGetFinished.WaitOne();

            otherIn.ChangeName(storageKey);
            new Thread(() => WriteData(storageKey, testData, true)).Start();
            new Thread(() => ReadData(otherIn, testData, true)).Start();
            isGetFinished.WaitOne();

            Assert.IsFalse(otherIn.TryGet(out outTestData));
        }

        [Test]
        public void TestChangeNameSetDataBeforeChangeName()
        {
            const string otherKey = "otherStorageKye";
            List<TestData> otherData = TestData.CreateData(100);

            WriteData(storageKey, testData);

            var otherIn = new OptimusPrimeIn(otherKey, stabService);
            new Thread(() => WriteData(otherKey, otherData, true)).Start();
            new Thread(() => ReadData(otherIn, otherData, true)).Start();
            isGetFinished.WaitOne();

            otherIn.ChangeName(storageKey);
            ReadData(otherIn, testData);

            Assert.IsFalse(otherIn.TryGet(out outTestData));
        }

        [Test]
        public void TestGet()
        {
            new Thread(() => WriteData(storageKey, testData, true)).Start();
            new Thread(() => ReadData(optimusPrimeIn, testData, true)).Start();
            isGetFinished.WaitOne();

            Assert.IsFalse(optimusPrimeIn.TryGet(out outTestData));
        }

        [Test]
        public void TestGetAfterTryGet()
        {
            WriteData(storageKey, testData);
            foreach (TestData data in testData)
            {
                Assert.IsTrue(optimusPrimeIn.TryGet(out outTestData));
                data.AssertAreEqual(outTestData);
            }

            var otherData = TestData.CreateData(100);
            new Thread(() => WriteData(storageKey, otherData, true)).Start();
            new Thread(() => ReadData(optimusPrimeIn, otherData, true)).Start();
            isGetFinished.WaitOne();

            Assert.IsFalse(optimusPrimeIn.TryGet(out outTestData));
        }
        
        [Test]
        public void TestGetAfterGetRange()
        {
            WriteData(storageKey, testData);
            TestData[] actual = optimusPrimeIn.GetRange<TestData>();

            for (int i = 0; i < testData.Count; i++)
                testData[i].AssertAreEqual(actual[i]);

            var otherData = TestData.CreateData(100);
            new Thread(() => WriteData(storageKey, otherData, true)).Start();
            new Thread(() => ReadData(optimusPrimeIn, otherData, true)).Start();
            isGetFinished.WaitOne();

            Assert.IsFalse(optimusPrimeIn.TryGet(out outTestData));
        }

        [Test]
        public void TestGetRange()
        {
            WriteData(storageKey, testData);
            TestData[] actual = optimusPrimeIn.GetRange<TestData>();

            for (int i = 0; i < testData.Count; i++)
                testData[i].AssertAreEqual(actual[i]);

            Assert.IsFalse(optimusPrimeIn.TryGet(out outTestData));
        }

        [Test]
        public void TestGetRangeAfterGet()
        {
            var otherData = TestData.CreateData(100);
            new Thread(() => WriteData(storageKey, otherData, true)).Start();
            new Thread(() => ReadData(optimusPrimeIn, otherData, true)).Start();
            isGetFinished.WaitOne();

            WriteData(storageKey, testData);
            TestData[] actual = optimusPrimeIn.GetRange<TestData>();

            for (int i = 0; i < testData.Count; i++)
                testData[i].AssertAreEqual(actual[i]);

            Assert.IsFalse(optimusPrimeIn.TryGet(out outTestData));
        }

        [Test]
        public void TestGetRangeAfterTryGet()
        {
            var otherData = TestData.CreateData(100);
            WriteData(storageKey, otherData);
            foreach (TestData data in otherData)
            {
                Assert.IsTrue(optimusPrimeIn.TryGet(out outTestData));
                data.AssertAreEqual(outTestData);
            }

            WriteData(storageKey, testData);
            TestData[] actual = optimusPrimeIn.GetRange<TestData>();

            for (int i = 0; i < testData.Count; i++)
                testData[i].AssertAreEqual(actual[i]);

            Assert.IsFalse(optimusPrimeIn.TryGet(out outTestData));
        }

        [Test]
        public void TestTryGet()
        {
            WriteData(storageKey, testData);
            foreach (TestData data in testData)
            {
                Assert.IsTrue(optimusPrimeIn.TryGet(out outTestData));
                data.AssertAreEqual(outTestData);
            }

            Assert.IsFalse(optimusPrimeIn.TryGet(out outTestData));
        }

        [Test]
        public void TestTryGetAfterGet()
        {
            var otherData = TestData.CreateData(100);
            new Thread(() => WriteData(storageKey, otherData, true)).Start();
            new Thread(() => ReadData(optimusPrimeIn, otherData, true)).Start();
            isGetFinished.WaitOne();

            WriteData(storageKey, testData);
            foreach (TestData data in testData)
            {
                Assert.IsTrue(optimusPrimeIn.TryGet(out outTestData));
                data.AssertAreEqual(outTestData);
            }

            Assert.IsFalse(optimusPrimeIn.TryGet(out outTestData));
        }

        [Test]
        public void TestTryGetAfterGetGange()
        {
            var otherData = TestData.CreateData(100);
            WriteData(storageKey, otherData);
            TestData[] actual = optimusPrimeIn.GetRange<TestData>();

            for (int i = 0; i < testData.Count; i++)
                otherData[i].AssertAreEqual(actual[i]);

            WriteData(storageKey, testData);
            foreach (TestData data in testData)
            {
                Assert.IsTrue(optimusPrimeIn.TryGet(out outTestData));
                data.AssertAreEqual(outTestData);
            }

            Assert.IsFalse(optimusPrimeIn.TryGet(out outTestData));
        }
    }
}