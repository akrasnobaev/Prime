using OptimusPrimeTest.Prime;
using Prime;

namespace OptimusPrimeTest.OptimusPrime
{
    public class OptimusRegisterGenericServiceTest : RegisterGenericServiceBaseTest
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new PrimeFactory();
        }
    }
}