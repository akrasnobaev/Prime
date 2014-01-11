using OptimusPrime.Factory;

namespace OptimusPrimeTests.Generics
{
    public class PrimeListenerTest : ListenerTestBase
    {
        protected override IFactory CreateFactory()
        {
            return new OptimusPrimeFactory();
        }
    }
}