using Prime;

namespace OptimusPrimeTests.Generics
{
    public class LibertyIsolatorTest : IsolatorTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new LibertyFactory();
        }
    }
}