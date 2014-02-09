using System;
using System.Threading;
using NUnit.Framework;
using Prime;

namespace OptimusPrimeTest.Prime
{
    [TestFixture]
    public abstract class ForkTestBase
    {
        private const int DataCount = 3;
        protected abstract IPrimeFactory CreateFactory();

        [Test]
        public void TestFork()
        {
            var factory = CreateFactory();
            var chain = factory.CreateChain(new Func<TestData, TestData>(AddOne));
            var fork = chain.Fork();
            var reader = fork.Source.CreateReader();
            var testData = TestData.CreateData(DataCount);

            factory.Start();

            var action = fork.Chain.ToFunctionalBlock();

            foreach (var data in testData)
                action.Process(data);

            /// Ожидание окончания работы всех цепочек.
            Thread.Sleep(100);

            factory.Stop();

            var actual = reader.GetCollection();
            Assert.AreEqual(DataCount, actual.Length);
            for (int i = 0; i < DataCount; i++)
            {
                var expected = testData[i];
                expected.Number++;
                expected.AssertAreEqual(actual[i]);
            }
        }

        private TestData AddOne(TestData data)
        {
            data.Number++;
            return data;
        }
    }
}
