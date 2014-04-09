using Prime;

namespace OptimusPrimeTest
{
    public class FuncLibertySmartCloneFactoryTest : SmartCloneFactoryTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new FuncLibertyFactory();
        }
    }
}