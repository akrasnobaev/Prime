using OptimusPrimeTest.Prime;
using Prime;

namespace OptimusPrimeTest.FuncLibertyPrime
{
    public class FuncLibertyLinkChainToChainTest : LinkChainToChainTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new FuncLibertyFactory();
        }
    }
}