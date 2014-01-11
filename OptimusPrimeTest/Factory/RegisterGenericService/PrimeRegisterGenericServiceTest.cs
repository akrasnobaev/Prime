using OptimusPrime.Factory;

namespace OptimusPrimeTest.Factory
{
    public class PrimeRegisterGenericServiceTest : RegisterGenericServiceBaseTest
    {
        protected override IFactory CreateFactory()
        {
            return new OptimusPrimeFactory();
        }
    }
}