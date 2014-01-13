using Prime;

namespace OptimusPrimeTests.Templates.SourceReader
{
    public class LibertySourceReaderTest : SourceReaderTestBase
    {
        protected override IPrimeFactory CreateFactory()
        {
            return new LibertyFactory();
        }
    }
}