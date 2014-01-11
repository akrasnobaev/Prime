using OptimusPrime.Factory;

namespace OptimusPrimeTests.Generics
{
    public class PrimeLinkToEnumerableTest : LinkToEnumerableTestBase
    {
        protected override IFactory CreateFactory()
        {
            return new OptimusPrimeFactory();
        }
    }
}