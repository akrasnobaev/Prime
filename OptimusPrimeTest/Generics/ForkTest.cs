using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OptimusPrime.Factory;

namespace OptimusPrimeTests.Generics
{
    [TestFixture]
    public class ForkTest
    {
        void Test(IFactory factory)
        {
            var chain = factory.CreateChain(new Func<int, int>(z => z + 1));
            var fork = chain.Fork();
            var reader = fork.Source.CreateReader();
            factory.Start();

            var action=fork.Chain.ToFunctionalBlock();

            for (int i = 0; i < 100; i++)
                action.Process(i);

            var collection = reader.GetCollection();
            Assert.AreEqual(100, collection.Length);
            for (int i = 0; i < 100; i++)
                Assert.AreEqual(i + 1, collection[i]);
        }

        [Test]
        public void ForkOptimus()
        {
            Test(new OptimusPrimeFactory());
        }

        [Test]
        public void ForkLiberty()
        {
            Test(new CallFactory());
        }
    }
}
