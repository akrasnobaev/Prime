using NUnit.Framework;
using OptimusPrime.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptimusPrimeTests.Generics
{
    [TestFixture]
    class ListenerTest
    {
        void Test(IFactory factory)
        {
            var producer = new DataProducer<int>();
            var source = factory.CreateSource(producer.SourceBlock);
            var array = new List<int>();
            source.Listen(z => { array.Add(z); });
            factory.Start();
            producer.Start(1000, 0, z => z);
            factory.Stop();
            Assert.AreEqual(1000, array.Count);
        }

        [Test]
        public void ListenerLiberty()
        {
            Test(new CallFactory());
        }
    }
}
