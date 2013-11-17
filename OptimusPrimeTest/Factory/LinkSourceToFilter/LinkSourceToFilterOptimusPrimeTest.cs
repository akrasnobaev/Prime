using NUnit.Framework;
using OptimusPrime.Factory;

namespace OptimusPrimeTests.Factory.LinkSourceToFilter
{
    public class LinkSourceToFilterOptimusPrimeTest : LinkSourceToFilterBaseTest
    {
        private OptimusPrimeFactory optimusPrimeFactory;

        [SetUp]
        public void SetUp()
        {
            optimusPrimeFactory = new OptimusPrimeFactory();
            base.SetUp();
        }

        protected override IFactory CreaFactory()
        {
            return optimusPrimeFactory;
        }
    }
}