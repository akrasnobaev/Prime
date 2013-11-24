using OptimusPrime.OprimusPrimeCore;

namespace OptimusPrimeTest
{
    public class WriteService<T> : OptimusPrimeService
    {
        private readonly T[] _testDatCollection;

        public WriteService(T[] testDatCollection)
            : base(TestConstants.Host, TestConstants.DbPage)
        {
            _testDatCollection = testDatCollection;
            OptimusPrimeOut = new IOptimusPrimeOut[] { new OptimusPrimeOut(TestConstants.StorageKey, this) };
        }

        public override void Initialize()
        {
        }

        public override void DoWork()
        {
            foreach (T testData in _testDatCollection)
                OptimusPrimeOut[0].Set(testData);
        }
    }
}