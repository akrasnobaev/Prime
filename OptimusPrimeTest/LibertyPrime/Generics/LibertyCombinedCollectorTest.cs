using OptimusPrimeTest.Prime;
using Prime;

namespace OptimusPrimeTest.LibertyPrime
{
    public class LibertyCombinedCollectorTest : CombinedCollectorTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new LibertyFactory();
        }
    }
}