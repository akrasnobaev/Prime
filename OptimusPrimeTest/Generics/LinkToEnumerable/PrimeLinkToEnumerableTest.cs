using Prime;

namespace OptimusPrimeTests.Generics
{
    public class PrimeLinkToEnumerableTest : LinkToEnumerableTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new PrimeFactory();
        }
    }
}