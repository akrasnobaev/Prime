using OptimusPrimeTest.Prime;
using Prime;

namespace OptimusPrimeTest.OptimusPrime
{
    public class OptimusLinkToEnumerableTest : LinkToEnumerableTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new PrimeFactory();
        }
    }
}