using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OptimusPrime.Factory;
using OptimusPrime.Templates;
using OptimusPrime;
using OptimusPrime.Generics;

namespace OptimusPrimeTests.Generics
{
    [TestFixture]
    class ForkRepeaterTest
    {
     
        class RepeaterForFork : IRepeaterBlock<string, string, char, char, char>
        {
            string input;
            int position;
            

            public void Start(string publicIn)
            {
                input = publicIn;
                position = 0;
            }

            public bool MakeIteration(char sourceDatas, char oldPrivateOut, out char privateIn)
            {
                Assert.NotNull(sourceDatas);
                if (position != 0)
                    Assert.AreEqual(oldPrivateOut, sourceDatas);
                
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
            var collector = fork.Source.CreateSyncCollector().CreateRepeaterAdapter();
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
