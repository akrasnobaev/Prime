using OptimusPrime.Factory;

namespace OptimusPrimeTests.Generics
{
    public class LibertyForkTest : ForkTestBase
    {
        protected override IFactory CreateFactory()
        {
            return new CallFactory();
        }
    }
}