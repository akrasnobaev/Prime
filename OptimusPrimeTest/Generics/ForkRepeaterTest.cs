using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OptimusPrime.Factory;
using OptimusPrime.Templates;
using OptimusPrime;

namespace OptimusPrimeTests.Generics
{
    [TestFixture]
    class ForkRepeaterTest
    {
        class CharSourceCollection : SyncronousSourceDataCollection<char> {}

        class RepeaterForFork : IRepeaterBlock<string, string, char, char, CharSourceCollection>
        {
            string input;
            int position;
            

            public void Start(string publicIn)
            {
                input = publicIn;
                position = 0;
            }

            public bool MakeIteration(CharSourceCollection sourceDatas, char oldPrivateOut, out char privateIn)
            {
                Assert.NotNull(sourceDatas);
                if (position != 0)
                    Assert.AreEqual(oldPrivateOut, sourceDatas.Data0);
                
                if (position >= input.Length)
                {
                    privateIn = '\0';
                    return false;
                }

                privateIn = input[position];
                position++;
                return true;
            }

            public string Conclude()
            {
                return "";
            }
        }

        void TestForkRepeater(IFactory factory)
        {
            var privateChain = factory.CreateChain(new Func<char, char>(c => c));
            var fork = privateChain.Fork();
            var collector = factory.BindSources(fork.Source).CreateSyncCollector<CharSourceCollection>();
            var repeater = factory.CreateRepeater(new RepeaterForFork(), collector, fork.Chain);
            factory.Start();
            repeater.ToFunctionalBlock().Process("abcdefghij");
            factory.Stop();
        }

        [Test]
        public void ForkRepeaterLiberty()
        {
            TestForkRepeater(new CallFactory());
        }

        [Test]
        public void ForkRepeaterOptimus()
        {
            TestForkRepeater(new OptimusPrimeFactory());
        }
    }
}
