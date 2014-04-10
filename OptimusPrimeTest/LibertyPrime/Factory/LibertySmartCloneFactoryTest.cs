using Prime;

namespace OptimusPrimeTest.LibertyPrime
{
    public class LibertySmartCloneFactoryTest : SmartCloneFactoryTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new LibertyFactory();
        }
    }
}