using OptimusPrime.Factory;

namespace OptimusPrimeTests.Generics.Fork
{
    public class LibertyForkTest : ForkTestBase
    {
        protected override IFactory CreateFactory()
        {
            return new CallFactory();
        }
    }
}