using OptimusPrimeTest.Prime;
using Prime;

namespace OptimusPrimeTest.OptimusPrime
{
    public class OptimusListenerTest : ListenerTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new PrimeFactory();
        }
    }
}