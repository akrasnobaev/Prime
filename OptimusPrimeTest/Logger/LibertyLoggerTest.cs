using Prime;

namespace OptimusPrimeTest.Logger
{
    public class LibertyLoggerTest : LoggerTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new LibertyFactory();
        }
    }
}