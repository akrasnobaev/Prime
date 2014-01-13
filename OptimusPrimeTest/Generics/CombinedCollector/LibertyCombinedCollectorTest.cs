using Prime;

namespace OptimusPrimeTests.Generics
{
    public class LibertyCombinedCollectorTest : CombinedCollectorTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new LibertyFactory();
        }
    }
}