using OptimusPrime.Factory;

namespace OptimusPrimeTest.Factory.RegisterGenericService
{
    public class RegisterGenericServicePrimeTest : RegisterGenericServiceBaseTest
    {
        protected override IFactory CreateFactory()
        {
            return new OptimusPrimeFactory();
        }
    }
}