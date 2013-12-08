using OptimusPrime.Factory;

namespace OptimusPrimeTest.Factory.RegisterGenericService
{
    public class RegisterGenericServiceLibertyTest : RegisterGenericServiceBaseTest
    {
        protected override IFactory CreateFactory()
        {
            return new CallFactory();
        }
    }
}