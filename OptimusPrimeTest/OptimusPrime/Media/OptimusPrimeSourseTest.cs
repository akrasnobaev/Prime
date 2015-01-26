using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OptimusPrimeTest.Prime;
using Prime;
using Prime.Optimus;

namespace OptimusPrimeTest.OptimusPrime
{
    [TestFixture]
    public class OptimusPrimeSourseTest
    {
        private const int DataCount = 3;

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
            var sourceData = TestData.CreateData(DataCount);
            var actualData = TestSourse(sourceData);
            Assert.AreEqual(sourceData.Count, actualData.Count);
            for (var i = 0; i < sourceData.Count; i++)
                sourceData[i].AssertAreEqual(actualData[i]);
        }

        private static IList<T> TestSourse<T>(IEnumerable<T> sourceData)
        {
            var factory = new PrimeFactory();
            var sourceBlock = new EventBlock<T>();
            var optimusPrimeSource = factory.CreateSource(sourceBlock) as OptimusSource<T>;

            factory.Start();
            var readService = new ReadService<T>(sourceData.Count(), optimusPrimeSource.Output.Name);
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