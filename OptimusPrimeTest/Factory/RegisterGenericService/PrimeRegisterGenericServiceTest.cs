using Prime;

namespace OptimusPrimeTest.Factory
{
    public class PrimeRegisterGenericServiceTest : RegisterGenericServiceBaseTest
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new PrimeFactory();
        }
    }
}