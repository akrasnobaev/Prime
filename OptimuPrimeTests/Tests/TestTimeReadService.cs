using System.Diagnostics;
using OptimusPrime.OprimusPrimeCore.Service;

namespace OptimuPrimeTests.Tests
{
    public class TestTimeReadService : OptimusPrimeService
    {
        public TestTimeReadService(string host = "localhost", int dbPage = 1) : base(host, dbPage)
        {
            OptimusPrimeIn = new IOptimusPrimeIn[] { new OptimusPrimeIn("TestKey1", this) };
            OptimusPrimeOut = new IOptimusPrimeOut[0];
        }

        public override void Actuation()
        {
            for (var i = 0; i < 1000; i++)
            {
                OptimusPrimeIn[0].Get<byte[]>();
                Debug.WriteLine("Read: {0}", i);
            }
        }
    }
}