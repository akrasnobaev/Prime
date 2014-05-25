using System;
using NUnit.Framework;
using OptimusPrime.Common.Exception;
using OptimusPrimeTest.Prime;
using Prime;

namespace OptimusPrimeTest.LibertyPrime.Chain
{
    [TestFixture]
    public class LibertyHandlesExceptionChainTest
    {
        private IHandlesExceptionChain<TestData, TestData> chain;
        private LibertyFactory factory;

        [SetUp]
        public void BeforeEach()
        {
            factory = new LibertyFactory();
            Func<TestData, TestData> func = data =>
            {
                data.Number++;
                return data;
            };
            chain = factory.CreateHandlesExceptionChain(func);
        }

        [Test, ExpectedException(typeof (ChainAlreadyUsedException))]
        public void TestChainToFunctionalBlockTwice()
        {
            // first use
            chain.ToFunctionalBlock();

            // second use
            chain.ToFunctionalBlock();
        }

        [Test]
        public void TestCloneOnToFunctionalBlock()
        {
            var testDatas = TestData.CreateData(2);
            var block = chain.ToFunctionalBlock();

            foreach (var testData in testDatas)
            {
                var originalData = testData.Clone();
                var resultData = block.Process(testData);
                // Изменяем источник данных, для проверки работы клона
                testData.Name = "newName";
                resultData.Number--;
                originalData.AssertAreEqual(resultData);
            }
        }

        [Test]
        public void TestToFunctionalBlockWithoutError()
        {
            var testDatas = TestData.CreateData(2);
            var block = chain.ToFunctionalBlock();

            foreach (var testData in testDatas)
            {
                var resultData = block.Process(testData);
                resultData.Number--;
                testData.AssertAreEqual(resultData);
            }
        }

        [Test]
        public void TestToFunctionalBlockWithErrorAndHandler()
        {
            var testDatas = TestData.CreateData(2);
            chain = factory.CreateHandlesExceptionChain(
                new Func<TestData, TestData>(data => { throw new TestException("message", data); }));
            chain.HandleExceptions(exception =>
            {
                var testException = (TestException) exception;
                return testException.TestData;
            });
            var block = chain.ToFunctionalBlock();

            foreach (var testData in testDatas)
            {
                var resultData = block.Process(testData);
                testData.AssertAreEqual(resultData);
            }
        }

        [Test, ExpectedException(typeof (TestException))]
        public void TestToFunctionalBlockWithErrorAndWithoutHandler()
        {
            var testDatas = TestData.CreateData(1);
            chain = factory.CreateHandlesExceptionChain(
                new Func<TestData, TestData>(data => { throw new TestException("message", data); }));
            var block = chain.ToFunctionalBlock();

            foreach (var testData in testDatas)
            {
                block.Process(testData);
            }
        }
    }
}