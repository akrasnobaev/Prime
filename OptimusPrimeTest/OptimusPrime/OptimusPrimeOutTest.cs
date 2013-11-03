using NUnit.Framework;
using OptimusPrime.OprimusPrimeCore;

namespace OptimusPrimeTest
{
    [TestFixture]
    public class OptimusPrimeOutTest
    {
        private OptimusPrimeIn optimusPrimeIn;
        private OptimusPrimeOut optimusPrimeOut;
        private const string storageKey = "storageKey";

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
            var first = new TestData {Name = "first", Number = 1};
            var second = new TestData {Name = "first", Number = 1};

            Assert.IsFalse(optimusPrimeIn.TryGet(out testData));

            optimusPrimeOut.Set(first);
            optimusPrimeOut.Set(second);

            first.AssertAreEqual(optimusPrimeIn.Get<TestData>());
            second.AssertAreEqual(optimusPrimeIn.Get<TestData>());

            Assert.IsFalse(optimusPrimeIn.TryGet(out testData));
        }
    }
}