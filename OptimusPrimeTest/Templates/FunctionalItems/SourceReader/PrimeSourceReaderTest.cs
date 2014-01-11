using OptimusPrime.Factory;

namespace OptimusPrimeTests.Templates.SourceReader
{
    public class PrimeSourceReaderTest : SourceReaderTestBase
    {
        protected override IFactory CreateFactory()
        {
            return new OptimusPrimeFactory();
        }
    }
}