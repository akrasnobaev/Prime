using System.Collections.Generic;
using System.Threading;
using Eurobot.Services;
using NUnit.Framework;
using OptimusPrime.Factory;
using OptimusPrime.OprimusPrimeCore;
using OptimusPrime.Templates;
using System.Linq;

namespace OptimusPrimeTest.Logger
{
    [TestFixture]
    public abstract class LoggerTestBase
    {
        private List<TestData> _testDatas;
        private OptimusPrimeLogger _logger;
        private ISource<TestData> _source;
        private IChain<TestData, TestData> _chain;
        protected abstract IFactory Factory { get; }
        private const int DataCount = 3;
        private const string SourcePseudoName = "sourcePseudoName";
        private const string ChainPseudoName = "chainPseudoName";

        protected void Setup()
        {
            var sourceBlock = new SourceBlock<TestData>();
            _source = Factory.CreateSource(sourceBlock, SourcePseudoName);
            _chain = Factory.CreateChain<TestData, TestData>(MultipleNumber, ChainPseudoName);
            Factory.LinkSourceToChain(_source, _chain);

            Factory.Start();

            _testDatas = TestData.CreateData(DataCount);
            foreach (TestData data in _testDatas)
                sourceBlock.Publish(data);

            Thread.Sleep(100);
            Factory.Stop();

            _logger = new OptimusPrimeLogger();

            string filePath = Factory.DumpDb();
            _logger.LoadFile(filePath);
        }

        [Test]
        public void TestGet()
        {
            foreach (TestData data in _testDatas)
            {
                var sourceData = _logger.Get<TestData>(_source.Name);
                var chainData = _logger.Get<TestData>(_chain.OutputName);

                data.AssertAreEqual(sourceData);
                MultipleNumber(data).AssertAreEqual(chainData);
            }

            TestData testData;
            Assert.IsFalse(_logger.TryGet(_source.Name, out testData));
            Assert.IsFalse(_logger.TryGet(_chain.OutputName, out testData));
        }

        [Test]
        public void TestGetRange()
        {
            var sourceDatas = _logger.GetRange<TestData>(_source.Name).ToArray();
            var chainDatas = _logger.GetRange<TestData>(_chain.OutputName).ToArray();
            for (int i = 0; i < _testDatas.Count; i++)
            {
                _testDatas[i].AssertAreEqual(sourceDatas[i]);
                MultipleNumber(_testDatas[i]).AssertAreEqual(chainDatas[i]);
            }

            TestData testData;
            Assert.IsFalse(_logger.TryGet(_source.Name, out testData));
            Assert.IsFalse(_logger.TryGet(_chain.OutputName, out testData));
        }

        [Test]
        public void TestGetRangeaAfterGet()
        {
            _testDatas[0].AssertAreEqual(_logger.Get<TestData>(_source.Name));
            MultipleNumber(_testDatas[0]).AssertAreEqual(_logger.Get<TestData>(_chain.OutputName));
            var start = 1; // количетво прочитанных данных;

            var sourceDatas = _logger.GetRange<TestData>(_source.Name).ToArray();
            var chainDatas = _logger.GetRange<TestData>(_chain.OutputName).ToArray();
            for (int i = start; i < _testDatas.Count; i++)
            {
                _testDatas[i].AssertAreEqual(sourceDatas[i - start]);
                MultipleNumber(_testDatas[i]).AssertAreEqual(chainDatas[i - start]);
            }

            TestData testData;
            Assert.IsFalse(_logger.TryGet(_source.Name, out testData));
            Assert.IsFalse(_logger.TryGet(_chain.OutputName, out testData));
        }

        [Test]
        public void TestTryGet()
        {
            foreach (TestData data in _testDatas)
            {
                TestData sourceData, chainData;


                Assert.IsTrue(_logger.TryGet(_source.Name, out sourceData));
                Assert.IsTrue(_logger.TryGet(_chain.OutputName, out chainData));

                data.AssertAreEqual(sourceData);
                MultipleNumber(data).AssertAreEqual(chainData);
            }

            TestData testData;
            Assert.IsFalse(_logger.TryGet(_source.Name, out testData));
            Assert.IsFalse(_logger.TryGet(_chain.OutputName, out testData));
        }

        [Test]
        public void TestPseudoName()
        {
            foreach (TestData data in _testDatas)
            {
                var sourceData = _logger.Get<TestData>(SourcePseudoName);
                var chainData = _logger.Get<TestData>(ChainPseudoName);

                data.AssertAreEqual(sourceData);
                MultipleNumber(data).AssertAreEqual(chainData);
            }

            TestData testData;
            Assert.IsFalse(_logger.TryGet(SourcePseudoName, out testData));
            Assert.IsFalse(_logger.TryGet(ChainPseudoName, out testData));
        }

        private TestData MultipleNumber(TestData data)
        {
            return new TestData(data.Id, data.Name, data.Number*2);
        }
    }
}