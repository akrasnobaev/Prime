using OptimusPrime.Factory;

namespace OptimusPrimeTest.Factory
{
    public class LibertyLinkSourceToChainTest : LinkSourceToChainBaseTest
    {
        protected override IFactory CreaFactory()
        {
            return new CallFactory();
        }
    }
}