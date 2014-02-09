using OptimusPrimeTest.Prime;
using Prime;

namespace OptimusPrimeTest.OptimusPrime
{
    public class OptimusCombinedCollectorTest : CombinedCollectorTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new PrimeFactory();
        }
    }
}