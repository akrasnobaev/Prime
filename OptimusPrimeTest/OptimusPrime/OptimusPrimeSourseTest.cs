using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Eurobot.Services;
using NUnit.Framework;
using OptimusPrime.Factory;
using OptimusPrime.Templates;

namespace OptimusPrimeTest
{
    [TestFixture]
    public class OptimusPrimeSourseTest
    {
        [Test]
        public void TestSourseInt()
        {
            var sourceData = new[] {27, 13, 1};
            IList<int> actualData = TestSourse(sourceData);
            CollectionAssert.AreEqual(sourceData, actualData.ToArray());
        }

        [Test]
        public void TestSourseString()
        {
            var sourceData = new[] {"foo", "bar", "lol"};
            IList<string> actualData = TestSourse(sourceData);
            CollectionAssert.AreEqual(sourceData, actualData.ToArray());
        }

        [Test]
        public void TestSourseComplexData()
        {
            var sourceData = new[]
                {
                    new TestData {Name = "foo", Number = 1},
                    new TestData {Name = "bar", Number = 2},
                    new TestData {Name = "lol", Number = 3}
                };
            IList<TestData> actualData = TestSourse(sourceData);
            for (int i = 0; i < actualData.Count; i++)
            {
                Assert.AreEqual(sourceData[i].Id, actualData[i].Id);
                Assert.AreEqual(sourceData[i].Name, actualData[i].Name);
                Assert.AreEqual(sourceData[i].Number, actualData[i].Number);
            }
        }

        private static IList<T> TestSourse<T>(T[] sourceData)
        {
            var factory = new OptimusPrimeFactory();
            var sourceBlock = new SourceBlock<T>();
            var optimusPrimeSource = factory.CreateSource(sourceBlock) as OptimusPrimeSource<T>;

            factory.Start();
            var readService = new ReadService<T>(sourceData.Length, optimusPrimeSource.Output.Name);
            var testThread = new Thread(readService.DoWork);
            testThread.Start();

            foreach (T data in sourceData)
                sourceBlock.Publish(data);
            factory.Stop();
            readService.AutoResetEvent.WaitOne();
            testThread.Abort();

            return readService.TestDataCollection;
        }
    }
}