using Prime;

namespace OptimusPrimeTests.Templates.SourceReader
{
    public class PrimeSourceReaderTest : SourceReaderTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new PrimeFactory();
        }
    }
}