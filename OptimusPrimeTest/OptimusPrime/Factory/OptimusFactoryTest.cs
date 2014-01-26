using OptimusPrimeTest.Prime;
using Prime;

namespace OptimusPrimeTest.OptimusPrime
{
    public class OptimusFactoryTest : FactoryTestBase
    {
        protected override IPrimeFactory CreateFactory(bool isLogging = true)
        {
            return new PrimeFactory(isLogging);
        }
    }
}