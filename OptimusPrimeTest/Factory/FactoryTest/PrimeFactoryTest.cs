using Prime;

namespace OptimusPrimeTest.Factory
{
    public class PrimeFactoryTest : FactoryTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new PrimeFactory();
        }
    }
}