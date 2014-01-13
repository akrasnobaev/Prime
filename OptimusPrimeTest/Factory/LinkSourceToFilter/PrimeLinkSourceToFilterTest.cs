using Prime;

namespace OptimusPrimeTest.Factory
{
    public class PrimeLinkSourceToFilterTest : LinkSourceToFilterBaseTest
    {
        protected override IPrimeFactory CreaFactory()
        {
            return new PrimeFactory();
        }
    }
}