using System.Threading;
using NUnit.Framework;
using Prime;

namespace OptimusPrimeTest.Prime
{
    public abstract class RegisterGenericServiceBaseTest
    {
        private IPrimeFactory factory;
        private TestGenericService genericService;
        protected abstract IPrimeFactory CreateFactory();

        [SetUp]
        public void SetUp()
        {
            factory = CreateFactory();
            genericService = new TestGenericService();

            factory.RegisterGenericService(genericService);
            factory.Start();
        }

        [TearDown]
        public void TearDown()
        {
            factory.Stop();
        }

        [Test]
        public void TestRegisterGenericServiceInitialized()
        {
            Assert.IsTrue(genericService.IsInitialize);
        }

        [Test]
        public void TestRegisterGenericServiceDoWorked()
        {
            // Нет синхронизации по началу работы generic-сервиса,
            // и делать ее не хочется, потому что при падении тест зависнет.
            Thread.Sleep(1);

            Assert.IsTrue(genericService.IsDoWork);
        }
    }
}