using OptimusPrimeTest.Prime;
using Prime;

namespace OptimusPrimeTest.LibertyPrime
{
    public class LibertyLinkToEnumerableTest : LinkToEnumerableTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new LibertyFactory();
        }
    }
}