using NUnit.Framework;
using OptimusPrime.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptimusPrimeTests.Generics
{
    [TestFixture]
    class LinkToEnumerableTest
    {
        void Test(IFactory factory)
        {
            int OneToMany=10;
            int count = 1000;

            var producer = new DataProducer<int>();
            var source = factory.CreateSource(producer.SourceBlock);
            var source1 = source.LinkToEnumerable(
                z => Enumerable.Range(0,OneToMany).Select(x=>z)
                );
            var array = new List<int>();
            source1.Listen(z => { array.Add(z); });
            factory.Start();
            producer.Start(count, 3, z=>z);
            factory.Stop();
            Assert.AreEqual(count*OneToMany, array.Count);
            for (int i = 0; i < array.Count; i++)
                Assert.AreEqual(i / OneToMany, array[i]);
        }

        [Test]
        public void LinkToEnumerableLiberty()
        {
            Test(new CallFactory());
        }
    }
}
