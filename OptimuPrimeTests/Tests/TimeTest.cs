using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using BookSleeve;
using NUnit.Framework;

namespace OptimuPrimeTests.Tests
{
    [TestFixture]
    public class TimeTest
    {
        private RedisConnection connection;
        private const string TestKey = "TestKey1";

        [SetUp]
        public void Setup()
        {
            connection = new RedisConnection("localhost");
            var openTask = connection.Open();
            connection.Wait(openTask);
        }

        [TearDown]
        public void Teardown()
        {
            connection.Keys.Remove(1, TestKey);
        }

        [Test]
        public void TestRead([Values(
            1,
            10,
            100,
            512,
            1024)] int size)
        {
            var value = GenerateValue(size*1024);
            var testService = new TestTimeReadService();
            var stopwatch = new Stopwatch();

            for (var i = 0; i < 1000; i++)
            {
                var task = connection.Lists.AddLast(1, TestKey, value);
                connection.Wait(task);
            }

            stopwatch.Start();
            testService.Actuation();
            stopwatch.Stop();

            Debug.WriteLine("Total time: {0}, time by operation {1}", stopwatch.ElapsedMilliseconds,
                            ((double) stopwatch.ElapsedMilliseconds)/1000.0);
        }

        [Test]
        public void TestWrite([Values(
            1,
            10,
            100,
            512,
            1024)] int size)
        {
            var value = GenerateValue(size * 1024);
            var testService = new TestTimeWriteService(value);
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            testService.Actuation();
            stopwatch.Stop();

            Debug.WriteLine("Total time: {0}, time by operation {1}", stopwatch.ElapsedMilliseconds,
                            ((double)stopwatch.ElapsedMilliseconds) / 1000.0);
        }

        [Test]
        public void TestWriteRead([Values(
            1,
            10,
            100,
            512,
            1024)] int size)
        {
            var value = GenerateValue(size * 1024);
            var timeWriteService = new TestTimeWriteService(value);
            var timeReadService = new TestTimeReadService();
            var stopwatch = new Stopwatch();

            var timeWriteServiceThread = new Thread(timeWriteService.Actuation);
            var timeReadServiceThread = new Thread(timeReadService.Actuation);

            stopwatch.Start();
            timeWriteServiceThread.Start();
            timeReadServiceThread.Start();

            timeWriteServiceThread.Join();
            timeReadServiceThread.Join();
            stopwatch.Stop();

            Debug.WriteLine("Total time: {0}, time by operation {1}", stopwatch.ElapsedMilliseconds,
                            ((double)stopwatch.ElapsedMilliseconds) / 1000.0);
        }

        private byte[] GenerateValue(long size)
        {
            var bytes = new byte[size];
            for (var i = 0; i < size; i++)
                bytes[i] = (byte) (i%256);

            var binaryFormatter = new BinaryFormatter();
            var memoryStream = new MemoryStream();

            binaryFormatter.Serialize(memoryStream, bytes);
            return memoryStream.ToArray();
        }
    }
}