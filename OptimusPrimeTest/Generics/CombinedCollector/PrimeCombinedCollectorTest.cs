using OptimusPrime.Factory;

namespace OptimusPrimeTests.Generics
{
    public class PrimeCombinedCollectorTest : CombinedCollectorTestBase
    {
        protected override IFactory CreateFactory()
        {
            return new OptimusPrimeFactory();
        }
    }
}