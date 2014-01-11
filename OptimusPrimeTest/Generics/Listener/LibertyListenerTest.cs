using OptimusPrime.Factory;

namespace OptimusPrimeTests.Generics
{
    public class LibertyListenerTest : ListenerTestBase
    {
        protected override IFactory CreateFactory()
        {
            return new CallFactory();
        }
    }
}