using Prime;

namespace OptimusPrimeTests.Generics
{
    public class PrimeForkRepeaterTest : ForkRepeaterTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new PrimeFactory();
        }
    }
}