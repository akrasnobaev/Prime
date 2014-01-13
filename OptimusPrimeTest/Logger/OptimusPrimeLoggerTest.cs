using Prime;

namespace OptimusPrimeTest.Logger
{
    public class OptimusPrimeLoggerTest : LoggerTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new PrimeFactory();
        }
    }
}