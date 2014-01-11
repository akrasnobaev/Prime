using OptimusPrime.Factory;

namespace OptimusPrimeTest.Factory
{
    public class PrimeLinkSourceToChainTest : LinkSourceToChainBaseTest
    {
        protected override IFactory CreaFactory()
        {
            return new OptimusPrimeFactory();
        }
    }
}