using OptimusPrime.Factory;

namespace OptimusPrimeTests.Generics
{
    public class LibertyForkRepeaterTestBase : ForkRepeaterTestBase
    {
        protected override IFactory CreateFactory()
        {
            return new CallFactory();
        }
    }
}