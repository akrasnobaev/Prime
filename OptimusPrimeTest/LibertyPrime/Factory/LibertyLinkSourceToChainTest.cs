using OptimusPrimeTest.Prime;
using Prime;

namespace OptimusPrimeTest.LibertyPrime
{
    public class LibertyLinkSourceToChainTest : LinkSourceToChainTestBase
    {
        protected override IPrimeFactory CreaFactory()
        {
            return new LibertyFactory();
        }
    }
}