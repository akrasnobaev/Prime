using OptimusPrimeTest.Prime;
using Prime;

namespace OptimusPrimeTest.OptimusPrime
{
    public class OptimusLinkSourceToChainTest : LinkSourceToChainTestBase
    {
        protected override IPrimeFactory CreaFactory()
        {
            return new PrimeFactory();
        }
    }
}