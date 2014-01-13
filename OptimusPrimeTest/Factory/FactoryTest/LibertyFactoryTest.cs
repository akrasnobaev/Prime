using Prime;

namespace OptimusPrimeTest.Factory
{
    public class LibertyFactoryTest : FactoryTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new LibertyFactory();
        }
    }
}