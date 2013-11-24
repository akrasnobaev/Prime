using NUnit.Framework;
using OptimusPrime.Factory;

namespace OptimusPrimeTests.Factory.LinkSourceToChain
{
    public class LinkSourceToChainOptimusPrimeTest : LinkSourceToChainBaseTest
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