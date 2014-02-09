using OptimusPrimeTest.Prime;
using Prime;

namespace OptimusPrimeTest.LibertyPrime
{
    public class LibertyLinkSourceToFilterTest : LinkSourceToFilterBaseTest
    {
        protected override IPrimeFactory CreaFactory()
        {
            return new LibertyFactory();
        }
    }
}