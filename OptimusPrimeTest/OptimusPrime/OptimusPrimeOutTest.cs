using System.Collections.Generic;
using NUnit.Framework;
using OptimusPrime.OprimusPrimeCore;

namespace OptimusPrimeTest.Prime
{
    [TestFixture]
    public class OptimusPrimeOutTest
    {
        private OptimusPrimeIn optimusPrimeIn;
        private OptimusPrimeOut optimusPrimeOut;
        private const string storageKey = "storageKey";
        private const int DataCount = 3;

        [SetUp]
        public void Setup()
        {
            var stabService = new OptimusPrimeStabService(dbPage: 2);
            optimusPrimeIn = new OptimusPrimeIn(storageKey, stabService);
            optimusPrimeOut = new OptimusPrimeOut(storageKey, stabService);
        }

        [Test]
        public void TestSet()
        {
            TestData testData;
            var datas = TestData.CreateData(DataCount);

            Assert.IsFalse(optimusPrimeIn.TryGet(out testData));
            foreach (var data in datas)
                optimusPrimeOut.Set(data);

            foreach (var data in datas)
                optimusPrimeIn.Get<TestData>().AssertAreEqual(data);

            Assert.IsFalse(optimusPrimeIn.TryGet(out testData));
        }
    }
}