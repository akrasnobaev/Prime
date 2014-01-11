using OptimusPrime.Factory;

namespace OptimusPrimeTests.Generics
{
    public class LibertyIsolatorTest : IsolatorTestBase
    {
        protected override IFactory CreateFactory()
        {
            return new CallFactory();
        }
    }
}