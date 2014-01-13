using Prime;

namespace OptimusPrimeTest.Factory
{
    public class LibertyLinkSourceToChainTest : LinkSourceToChainBaseTest
    {
        protected override IPrimeFactory CreaFactory()
        {
            return new LibertyFactory();
        }
    }
}