using OptimusPrime.Factory;

namespace OptimusPrimeTests.Generics
{
    public class PrimeForkTest : ForkTestBase
    {
        protected override IFactory CreateFactory()
        {
            return new OptimusPrimeFactory();
        }
    }
}