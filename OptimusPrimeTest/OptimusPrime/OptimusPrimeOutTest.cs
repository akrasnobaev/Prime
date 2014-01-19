using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookSleeve;
using NUnit.Framework;
using Prime;
using Prime.Optimus;

namespace OptimusPrimeTest.Prime
{
    [TestFixture]
    public class OptimusPrimeOutTest
    {
        private OptimusOut optimusOut;
        private OptimusStabService stabService;
        private Stopwatch stopwatch;
        private const string storageKey = "storageKey";
        private const int DataCount = 3;

        [SetUp]
        public void Setup()
        {
            stabService = new OptimusStabService(dbPage: 2);
            stopwatch = new Stopwatch();
            optimusOut = new OptimusOut(storageKey, stabService, stopwatch);
        }

        [TearDown]
        public void TearDown()
        {
            var connection = new RedisConnection("localhost", allowAdmin: true);

            Task openTask = connection.Open();
            connection.Wait(openTask);

            Task flushDbTask = connection.Server.FlushAll();
            connection.Wait(flushDbTask);
        }

        [Test]
        public void TestSet()
        {
            var optimusIn = new OptimusIn(storageKey, stabService);
            TestData testData;
            Assert.IsFalse(optimusIn.TryGet(out testData));

            var datas = TestData.CreateData(DataCount);
            foreach (var data in datas)
                optimusOut.Set(data);

            foreach (var data in datas)
                optimusIn.Get<TestData>().AssertAreEqual(data);

            Assert.IsFalse(optimusIn.TryGet(out testData));
        }

        [Test]
        public void TestTimeStamps()
        {
            var timeStampKey = ServiceNameHelper.GetTimeStampName(storageKey);
            TimeSpan result;
            var emptyRange = stabService.Connection.Lists.Range(stabService.DbPage, timeStampKey, 0, -1);
            var emptyBytes = stabService.Connection.Wait(emptyRange);
            var timeStamps = emptyBytes.Select(SerializeExtension.Deserialize<TimeSpan>);
            CollectionAssert.IsEmpty(timeStamps);

            var datas = TestData.CreateData(DataCount);
            const int waitTime = 5;
            stopwatch.Start();
            foreach (var data in datas)
            {
                Thread.Sleep(waitTime);
                optimusOut.Set(data);
            }

            var range = stabService.Connection.Lists.Range(stabService.DbPage, timeStampKey, 0, -1);
            var bytes = stabService.Connection.Wait(range);
            var timeStampList = bytes.Select(SerializeExtension.Deserialize<TimeSpan>).ToArray();
            Assert.AreEqual(DataCount, timeStampList.Length);

            var expectedTimeStamp = 0;
            for (int i = 0; i < DataCount; i++)
            {
                expectedTimeStamp += waitTime;
                Debug.WriteLine(timeStampList[i]);
                Assert.Less(expectedTimeStamp, timeStampList[i].TotalMilliseconds);
            }
        }
    }
}