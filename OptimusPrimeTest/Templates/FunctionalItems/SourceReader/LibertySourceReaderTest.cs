using OptimusPrime.Factory;

namespace OptimusPrimeTests.Templates.FunctionalItems.SourceReader
{
    public class LibertySourceReaderTest : SourceReaderTestBase
    {
        protected override IFactory CreateFactory()
        {
            return new CallFactory();
        }
    }
}