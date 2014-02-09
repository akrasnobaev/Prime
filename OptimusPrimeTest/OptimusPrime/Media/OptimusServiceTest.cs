using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookSleeve;
using NUnit.Framework;
using OptimusPrimeTest.Prime;

namespace OptimusPrimeTest.OptimusPrime
{
    [TestFixture]
    public class OptimusServiceTest
    {
        private const int DataCount = 3;

        [Test]
        public void TestSendAndReceiveInt()
        {
            var testData = new[] {10, 8, 27};
            IList<int> actual = TestSendAndReceiveDataInternal(testData);
            CollectionAssert.AreEqual(testData, actual.ToArray());
        }

        [Test]
        public void TestSendAndReceiveString()
        {
            var testData = new[] {"first", "second", "third"};
            IList<string> actual = TestSendAndReceiveDataInternal(testData);
            CollectionAssert.AreEqual(testData, actual.ToArray());
        }

        [Test]
        public void TestSendAndReceiveComplexData()
        {
            var testData = TestData.CreateData(DataCount);
            var actual = TestSendAndReceiveDataInternal(testData);
            Assert.AreEqual(testData.Count, actual.Count);
            for (var i = 0; i < testData.Count; i++)
                testData[i].AssertAreEqual(actual[i]);
        }

        private static IList<T> TestSendAndReceiveDataInternal<T>(IEnumerable<T> testData)
        {
            var connection = new RedisConnection(TestConstants.Host, allowAdmin: true);

            Task openTask = connection.Open();
            connection.Wait(openTask);

            Task flushDbTask = connection.Server.FlushAll();
            connection.Wait(flushDbTask);

            var firstService = new WriteService<T>(testData);
            var firstThread = new Thread(firstService.DoWork);

            var secondService = new ReadService<T>(testData.Count());
            var secondThread = new Thread(secondService.DoWork);

            firstThread.Start();
            secondThread.Start();

            secondService.AutoResetEvent.WaitOne();

            firstThread.Abort();
            secondThread.Abort();

            return secondService.TestDataCollection;
        }
    }
}