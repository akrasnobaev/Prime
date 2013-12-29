using OptimusPrime.Factory;

namespace OptimusPrimeTests.Templates.FunctionalItems.SourceReader
{
    public class PrimeSourceReaderTest : SourceReaderTestBase
    {
        protected override IFactory CreateFactory()
        {
            return new OptimusPrimeFactory();
        }
    }
}