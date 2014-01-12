using OptimusPrime.Factory;

namespace OptimusPrimeTests.Generics
{
    public class PrimeForkRepeaterTest : ForkRepeaterTestBase
    {
        protected override IFactory CreateFactory()
        {
            return new OptimusPrimeFactory();
        }
    }
}