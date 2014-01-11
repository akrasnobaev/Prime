using OptimusPrime.Factory;

namespace OptimusPrimeTest.Factory
{
    public class PrimeLinkSourceToFilterTest : LinkSourceToFilterBaseTest
    {
        protected override IFactory CreaFactory()
        {
            return new OptimusPrimeFactory();
        }
    }
}