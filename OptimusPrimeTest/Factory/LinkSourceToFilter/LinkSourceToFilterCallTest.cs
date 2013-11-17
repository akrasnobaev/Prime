using NUnit.Framework;
using OptimusPrime.Factory;

namespace OptimusPrimeTests.Factory.LinkSourceToFilter
{
    public class LinkSourceToFilterCallTest : LinkSourceToFilterBaseTest
    {
        private CallFactory callFactory;

        [SetUp]
        public void SetUp()
        {
            callFactory = new CallFactory();
            base.SetUp();
        }

        protected override IFactory CreaFactory()
        {
            return callFactory;
        }
    }
}