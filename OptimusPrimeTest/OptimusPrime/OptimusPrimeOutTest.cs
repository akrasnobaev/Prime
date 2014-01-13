using NUnit.Framework;
using Prime.Optimus;

namespace OptimusPrimeTest.Prime
{
    [TestFixture]
    public class OptimusPrimeOutTest
    {
        private OptimusIn _optimusIn;
        private OptimusOut _optimusOut;
        private const string storageKey = "storageKey";
        private const int DataCount = 3;

        [SetUp]
        public void Setup()
        {
            var stabService = new OptimusStabService(dbPage: 2);
            _optimusIn = new OptimusIn(storageKey, stabService);
            _optimusOut = new OptimusOut(storageKey, stabService);
        }

        [Test]
        public void TestSet()
        {
            TestData testData;
            var datas = TestData.CreateData(DataCount);

            Assert.IsFalse(_optimusIn.TryGet(out testData));
            foreach (var data in datas)
                _optimusOut.Set(data);

            foreach (var data in datas)
                _optimusIn.Get<TestData>().AssertAreEqual(data);

            Assert.IsFalse(_optimusIn.TryGet(out testData));
        }
    }
}