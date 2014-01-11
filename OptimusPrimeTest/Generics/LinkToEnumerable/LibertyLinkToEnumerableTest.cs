using OptimusPrime.Factory;

namespace OptimusPrimeTests.Generics
{
    public class LibertyLinkToEnumerableTest : LinkToEnumerableTestBase
    {
        protected override IFactory CreateFactory()
        {
            return new CallFactory();
        }
    }
}