using NUnit.Framework;
using OptimusPrime.Factory;

namespace OptimusPrimeTest.Logger
{
    public class CallLoggerTest : LoggerTestBase
    {
        private IFactory _factory;

        [SetUp]
        public new void Setup()
        {
            _factory = new CallFactory();
            base.Setup();
        }

        protected override IFactory Factory
        {
            get { return _factory; }
        }
    }
}