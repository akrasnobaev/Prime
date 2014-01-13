using Prime;

namespace OptimusPrimeTest.Factory
{
    public class LibertyRegisterGenericServiceTest : RegisterGenericServiceBaseTest
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new LibertyFactory();
        }
    }
}