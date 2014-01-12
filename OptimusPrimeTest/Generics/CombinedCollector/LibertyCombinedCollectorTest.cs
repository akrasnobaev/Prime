using OptimusPrime.Factory;

namespace OptimusPrimeTests.Generics
{
    public class LibertyCombinedCollectorTest : CombinedCollectorTestBase
    {
        protected override IFactory CreateFactory()
        {
            return new CallFactory();
        }
    }
}