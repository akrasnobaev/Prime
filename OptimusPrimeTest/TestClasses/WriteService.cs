using OptimusPrime.OprimusPrimeCore;

namespace OptimusPrimeTest
{
    public class WriteService<T> : OptimusPrimeService
    {
        private readonly T[] testDatCollection;

        public WriteService(T[] testDatCollection)
            : base(TestConstants.Host, TestConstants.DbPage)
        {
            this.testDatCollection = testDatCollection;
            OptimusPrimeOut = new IOptimusPrimeOut[] {new OptimusPrimeOut(TestConstants.StorageKey, this)};
        }

        public override void Actuation()
        {
            foreach (T testData in testDatCollection)
                OptimusPrimeOut[0].Set(testData);
        }
    }
}