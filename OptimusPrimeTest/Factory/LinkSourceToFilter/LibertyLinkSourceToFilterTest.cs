using Prime;

namespace OptimusPrimeTest.Factory
{
    public class LibertyLinkSourceToFilterTest : LinkSourceToFilterBaseTest
    {
        protected override IPrimeFactory CreaFactory()
        {
            return new LibertyFactory();
        }
    }
}