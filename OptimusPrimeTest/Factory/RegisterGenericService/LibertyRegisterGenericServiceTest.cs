using OptimusPrime.Factory;

namespace OptimusPrimeTest.Factory
{
    public class LibertyRegisterGenericServiceTest : RegisterGenericServiceBaseTest
    {
        protected override IFactory CreateFactory()
        {
            return new CallFactory();
        }
    }
}