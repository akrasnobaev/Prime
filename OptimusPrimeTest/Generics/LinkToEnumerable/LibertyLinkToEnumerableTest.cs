using Prime;

namespace OptimusPrimeTests.Generics
{
    public class LibertyLinkToEnumerableTest : LinkToEnumerableTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new LibertyFactory();
        }
    }
}