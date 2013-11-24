using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookSleeve;
using NUnit.Framework;

namespace OptimusPrimeTest
{
    [TestFixture]
    public class OptimusPrimeServiceTest
    {
        [SetUp]
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
            var testData = new[]
                {
                    new TestData {Name = "first", Number = 1},
                    new TestData {Name = "second", Number = 2},
                    new TestData {Name = "third", Number = 3}
                };
            IList<TestData> actual = TestSendAndReceiveDataInternal(testData);
            for (int i = 0; i < actual.Count; i++)
            {
                Assert.AreEqual(testData[i].Id, actual[i].Id);
                Assert.AreEqual(testData[i].Name, actual[i].Name);
                Assert.AreEqual(testData[i].Number, actual[i].Number);
            }
        }

        private static IList<T> TestSendAndReceiveDataInternal<T>(T[] testData)
        {
            var connection = new RedisConnection(TestConstants.Host, allowAdmin: true);

            Task openTask = connection.Open();
            connection.Wait(openTask);

            Task flushDbTask = connection.Server.FlushAll();
            connection.Wait(flushDbTask);

            var firstService = new WriteService<T>(testData);
            var firstThread = new Thread(firstService.DoWork);

            var secondService = new ReadService<T>(testData.Length);
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