using OptimusPrime.Generics;

namespace OptimusPrimeTest.Factory.RegisterGenericService
{
    public class TestGenericService : IGenericService
    {
        public bool IsInitialize { get; private set; }
        public bool IsDoWork{ get; private set; }

        public TestGenericService(string testString = "")
        {
            IsInitialize = false;
            IsDoWork = false;
        }

        public void Initialize()
        {
            IsInitialize = true;
        }

        public void DoWork()
        {
            IsDoWork = true;
        }
    }
}