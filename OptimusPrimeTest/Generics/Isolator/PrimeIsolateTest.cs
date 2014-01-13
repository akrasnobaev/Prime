using Prime;

namespace OptimusPrimeTests.Generics
{
    public class PrimeIsolateTest : IsolatorTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new PrimeFactory();
        }
    }
}