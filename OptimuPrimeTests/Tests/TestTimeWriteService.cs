using System.Diagnostics;
using OptimusPrime.OprimusPrimeCore.Service;

namespace OptimuPrimeTests.Tests
{
    public class TestTimeWriteService : OptimusPrimeService
    {
        private readonly byte[] value;

        public TestTimeWriteService(byte[] value, string host = "localhost", int dbPage = 1) : base(host, dbPage)
        {
            this.value = value;
            OptimusPrimeIn = new IOptimusPrimeIn[0];
            OptimusPrimeOut = new IOptimusPrimeOut[] { new OptimusPrimeOut("TestKey1", this) };
        }

        public override void Actuation()
        {
            for (var i = 0; i < 1000; i++)
            {
                OptimusPrimeOut[0].Set(value);
                Debug.WriteLine("Write {0}", i);
            }
        }
    }
}