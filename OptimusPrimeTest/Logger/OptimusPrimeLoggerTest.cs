using NUnit.Framework;
using OptimusPrime.Factory;

namespace OptimusPrimeTest.Logger
{
    public class OptimusPrimeLoggerTest : LoggerTestBase
    {
        private IFactory _factory;

        [SetUp]
        public new void Setup()
        {
            _factory = new OptimusPrimeFactory();
            base.Setup();
        }

        protected override IFactory Factory
        {
            get { return _factory; }
        }
    }
}