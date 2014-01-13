using Prime;

namespace OptimusPrimeTest.Factory
{
    public class PrimeLinkSourceToChainTest : LinkSourceToChainBaseTest
    {
        protected override IPrimeFactory CreaFactory()
        {
            return new PrimeFactory();
        }
    }
}