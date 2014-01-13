using Prime;

namespace OptimusPrimeTests.Generics
{
    public class PrimeListenerTest : ListenerTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new PrimeFactory();
        }
    }
}