using Prime;

namespace OptimusPrimeTests.Generics
{
    public class PrimeCombinedCollectorTest : CombinedCollectorTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new PrimeFactory();
        }
    }
}