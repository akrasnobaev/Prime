using OptimusPrime.Factory;

namespace OptimusPrimeTest.Factory
{
    public class PrimeFactoryTest : FactoryTestBase
    {
        protected override IFactory CreateFactory()
        {
            return new OptimusPrimeFactory();
        }
    }
}