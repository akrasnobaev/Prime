using System;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using OptimusPrime.Common.Exception;
using Prime;

namespace OptimusPrimeTest.Prime
{
    [TestFixture]
    public abstract class FactoryTestBase
    {
        /// <summary>
        /// Размер коллекции данных для тестирования.
        /// </summary>
        private const int DataCount = 3;

        /// <summary>
        /// Максимальное число попыток чтения из источника данных,
        /// поче чего тест фэйлится.
        /// </summary>
        private const int MaxTryReadCount = 100;

        /// <summary>
        /// Количество итерраций работы фабрики.
        /// </summary>
        private const int RepeatFactoryCount = 3;

        protected abstract IPrimeFactory CreateFactory(bool isLogging = true);

        [Test]
        public void TestFactorySingleStart()
        {
            var factory = CreateFactory();
            FactoryWork(factory);
        }

        [Test]
        public void TestFactoryMultipleStart()
        {
            for (int i = 0; i < RepeatFactoryCount; i++)
            {
                var factory = CreateFactory();
                FactoryWork(factory);
            }
        }

        [Test, ExpectedException(typeof (LoggingOffException))]
        public void TestLoggingOff()
        {
            var factory = CreateFactory(false);
            FactoryWork(factory);

            factory.DumpDb();
        }

        [Test, ExpectedException(typeof(ChainAlreadyUsedException))]
        public void TestTryUseAlreadyUsedChain()
        {
            var factory = CreateFactory();
            var chain = factory.CreateChain(new Func<TestData, TestData>(a => a));
            
            // first use
            chain.ToFunctionalBlock();

            // second use
            chain.ToFunctionalBlock();
        }

        /// <summary>
        /// Один цикл работы фабрики.
        /// </summary>
        private void FactoryWork(IPrimeFactory factory)
        {
            var sourceBlock = new EventBlock<TestData>();

            var source = factory.CreateSource(sourceBlock);
            var chain = factory.CreateChain<TestData, TestData>(inputData =>
            {
                inputData.Number *= 2;
                return inputData;
            });
            var secondChain = factory.CreateChain<TestData, TestData>(inputData =>
            {
                inputData.Number *= 2;
                return inputData;
            });
            var sourceOfDouble = factory.LinkSourceToChain(source, chain);
            var secondSource = factory.LinkSourceToChain(sourceOfDouble, secondChain);
            var reader = secondSource.Factory.CreateReciever(secondSource).GetReader();

            factory.Start();

            var testDatas = TestData.CreateData(DataCount);
            foreach (var testData in testDatas)
                sourceBlock.Publish(testData);

            foreach (var testData in testDatas)
            {
                TestData result;
                var tryCout = 0;

                while (!reader.TryGet(out result) && ++tryCout < MaxTryReadCount)
                    Thread.Sleep(10);

                Debug.WriteLine("Попыток чтения из источника {0}", tryCout);

                if (tryCout == MaxTryReadCount)
                    Assert.Fail("За {0} попыток не данные не были получены из источника.", MaxTryReadCount);

                testData.Number *= 4;
                testData.AssertAreEqual(result);
            }

            factory.Stop();
        }
    }
}