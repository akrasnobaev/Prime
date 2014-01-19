using OptimusPrimeTest.Prime;
using Prime;

namespace OptimusPrimeTest.OptimusPrime
{
    public class OptimusLinkSourceToFilterTest : LinkSourceToFilterBaseTest
    {
        protected override IPrimeFactory CreaFactory()
        {
            return new PrimeFactory();
        }
    }
}