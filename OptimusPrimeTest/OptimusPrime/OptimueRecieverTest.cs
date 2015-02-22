using OptimusPrimeTest.Prime;
using Prime;
using Prime.Liberty;

namespace OptimusPrimeTests.OptimusPrime
{
    class OptimueRecieverTest : RecieverTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return factory;
        }

        protected override IReciever<int> CreateMockReciever(string input)
        {
            return new MockLibertyReciever<int>(factory, input, factory.Collections[input]);
        }

        private LibertyFactory factory = new LibertyFactory(true);
    }
}
