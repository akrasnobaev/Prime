using OptimusPrime.Factory;

namespace OptimusPrimeTest.Factory
{
    public class LibertyFactoryTest : FactoryTestBase
    {
        protected override IFactory CreateFactory()
        {
            return new CallFactory();
        }
    }
}