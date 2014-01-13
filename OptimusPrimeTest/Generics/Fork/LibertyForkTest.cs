using Prime;

namespace OptimusPrimeTests.Generics
{
    public class LibertyForkTest : ForkTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new LibertyFactory();
        }
    }
}