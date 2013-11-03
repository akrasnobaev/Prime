using System.Diagnostics;
using System.Threading;
using Eurobot.Services;
using NUnit.Framework;
using OptimusPrime.Factory;

namespace OptimusPrimeTest.Factory
{
    [TestFixture]
    public class FactoryTest
    {
        /// <summary>
        /// Максимальное число попыток чтения из источника данных,
        /// поче чего тест фэйлится.
        /// </summary>
        private const int MaxTryReadCount = 100;

        /// <summary>
        /// Количество итерраций работы фабрики.
        /// </summary>
        private const int RepeatFactoryCount = 10;

        [Test]
        public void TestOptimusPrimeFactorySingleStart()
        {
            var factory = new OptimusPrimeFactory();
            FactoryWork(factory);
        }

        [Test]
        public void TestOptimusPrimeFactoryMultipleStart()
        {
            for (int i = 0; i < RepeatFactoryCount; i++)
            {
                var factory = new OptimusPrimeFactory();
                FactoryWork(factory);
            }
        }

        [Test]
        public void TestCallFactorySingleStart()
        {
            var factory = new CallFactory();
            FactoryWork(factory);
        }

        [Test]
        public void TestCallFactoryMultipleStart()
        {
            for (int i = 0; i < RepeatFactoryCount; i++)
            {
                var factory = new CallFactory();
                FactoryWork(factory);
            }
        }

        /// <summary>
        /// Один цикл работы фабрики.
        /// </summary>
        private void FactoryWork(IFactory factory)
        {
            var sourceBlock = new SourceBlock<TestData>();

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
            var reader = secondSource.CreateReader();

            factory.Start();

            var testDatas = TestData.CreateData(3);
            foreach (var testData in testDatas)
                sourceBlock.Publish(testData);

            foreach (var testData in testDatas)
            {
                TestData result;
                var tryCout = 0;

                while (!reader.TryGet(out result) && ++tryCout < MaxTryReadCount)
                    Thread.Sleep(100);

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