using OptimusPrime.Factory;

namespace OptimusPrimeTest.Logger
{
    public class LibertyLoggerTest : LoggerTestBase
    {
        protected override IFactory CreateFactory()
        {
            return new CallFactory();
        }
    }
}