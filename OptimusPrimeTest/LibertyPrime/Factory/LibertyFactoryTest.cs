using OptimusPrimeTest.Prime;
using Prime;

namespace OptimusPrimeTest.LibertyPrime
{
    public class LibertyFactoryTest : FactoryTestBase
    {
        protected override IPrimeFactory CreateFactory(bool isLogging = true)
        {
            return new LibertyFactory(isLogging);
        }
    }
}