using Prime;

namespace OptimusPrimeTests.Generics
{
    public class LibertyListenerTest : ListenerTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new LibertyFactory();
        }
    }
}