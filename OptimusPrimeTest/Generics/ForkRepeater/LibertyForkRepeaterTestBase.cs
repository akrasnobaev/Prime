using Prime;

namespace OptimusPrimeTests.Generics
{
    public class LibertyForkRepeaterTestBase : ForkRepeaterTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new LibertyFactory();
        }
    }
}