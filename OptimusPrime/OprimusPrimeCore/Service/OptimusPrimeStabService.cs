namespace OptimusPrime.OprimusPrimeCore
{
    public class OptimusPrimeStabService : OptimusPrimeService
    {
        public OptimusPrimeStabService(string host = "localhost", int dbPage = 1) : base(host, dbPage)
        {
            OptimusPrimeIn = new IOptimusPrimeIn[0];
            OptimusPrimeOut = new IOptimusPrimeOut[0];
        }

        public override void Actuation()
        {
        }
    }
}