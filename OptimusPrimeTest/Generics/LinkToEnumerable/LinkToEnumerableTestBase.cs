using System.Collections.Generic;
using Eurobot.Services;
using NUnit.Framework;
using OptimusPrime.Factory;
using OptimusPrimeTest;

namespace OptimusPrimeTests.Generics
{
    [TestFixture]
    public abstract class LinkToEnumerableTestBase
    {
        private const int DataCount = 3;
        protected abstract IFactory CreateFactory();

        [Test]
        public void TestLinkToEnumerable()
        {
            var factory = CreateFactory();
            var sourceBlock = new SourceBlock<TestData>();
            var firstSource = factory.CreateSource(sourceBlock);
            var secondSource = firstSource.LinkToEnumerable(bifurcatedData);
            var reader = secondSource.CreateReader();
            var testData = TestData.CreateData(DataCount);

            factory.Start();

            foreach (var data in testData)
            {
                sourceBlock.Publish(data);
                
                var firstExpected = data.Clone();
                firstExpected.Number *= 10;
                firstExpected.AssertAreEqual(reader.Get());

                var secondExpected = data.Clone();
                secondExpected.Number *= 20;
                secondExpected.AssertAreEqual(reader.Get());
            }

            TestData lastData;
            Assert.IsFalse(reader.TryGet(out lastData));
            factory.Stop();
            
        }

        private static IEnumerable<TestData> bifurcatedData(TestData arg)
        {
            var first = arg.Clone();
            first.Number *= 10;

            var second = arg.Clone();
            second.Number *= 20;

            return new[] {first, second};
        }
    }
}
