using System.Collections.Generic;
using NUnit.Framework;
using OptimusPrime.Common.Exception;
using Prime;

namespace OptimusPrimeTest.Prime
{
    [TestFixture]
    public abstract class LinkChainToChainTestBase
    {
        private IPrimeFactory factory;
        private List<TestData> testData;
        private IChain<TestData, TestData> firstChain;
        private IChain<TestData, TestData> secondChain;
        private const int DataCount = 3;

        [SetUp]
        public void SetUp()
        {
            factory = CreateFactory();
            testData = TestData.CreateData(DataCount);
            firstChain = factory.CreateChain<TestData, TestData>(data =>
            {
                data.Number++;
                return data;
            });
            secondChain = factory.CreateChain<TestData, TestData>(data =>
            {
                data.Name += '_';
                return data;
            });
        }

        protected abstract IPrimeFactory CreateFactory();

        [Test]
        public void TestLinkChainToChain()
        {
            var resultChain = factory.LinkChainToChain(firstChain, secondChain);
            var func = resultChain.ToFunctionalBlock();

            factory.Start();

            foreach (var data in testData)
            {
                var expected = data.Clone();
                expected.Number++;
                expected.Name += '_';
                var actual = func.Process(data);
                expected.AssertAreEqual(actual);
            }

            factory.Stop();
        }

        [Test, ExpectedException(typeof (ChainAlreadyUsedException))]
        public void TestTryUseAlreadyUsedChain()
        {
            // first use
            firstChain.ToFunctionalBlock();

            // second use
            factory.LinkChainToChain(firstChain, secondChain);
        }
    }
}