using OptimusPrime.Factory;

namespace OptimusPrimeTest.Factory
{
    public class LibertyLinkSourceToFilterTest : LinkSourceToFilterBaseTest
    {
        protected override IFactory CreaFactory()
        {
            return new CallFactory();
        }
    }
}