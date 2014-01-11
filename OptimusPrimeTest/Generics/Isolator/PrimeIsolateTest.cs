using OptimusPrime.Factory;

namespace OptimusPrimeTests.Generics
{
    public class PrimeIsolateTest : IsolatorTestBase
    {
        protected override IFactory CreateFactory()
        {
            return new OptimusPrimeFactory();
        }
    }
}