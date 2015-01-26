using OptimusPrimeTest.Prime;
using Prime;

namespace OptimusPrimeTest.OptimusPrime
{
    public class OptimusLinkSourceToChainTest : LinkSourceToChainTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new PrimeFactory();
        }
    }
}