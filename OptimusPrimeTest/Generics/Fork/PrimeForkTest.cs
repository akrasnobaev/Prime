using Prime;

namespace OptimusPrimeTests.Generics
{
    public class PrimeForkTest : ForkTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new PrimeFactory();
        }
    }
}