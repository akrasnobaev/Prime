using System;
using NUnit.Framework;
using Prime;

namespace OptimusPrimeTest.Prime
{
    [TestFixture]
    public abstract class ForkRepeaterTestBase
    {

     
        class RepeaterForFork : IRepeaterBlock<string, string, char, char, char>
        {
            private string input;
            private int position;


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

        [Test]
        public void TestForkRepeater()
        {
            var factory = CreateFactory();
            var privateChain = factory.CreateChain(new Func<char, char>(c => c));
            var fork = privateChain.Fork();
            var collector = fork.Source.CreateSyncCollector().CreateRepeaterAdapter();
            var repeater = factory.CreateRepeater(new RepeaterForFork(), collector, fork.Chain);
            factory.Start();
            repeater.ToFunctionalBlock().Process("abcdefghij");
            factory.Stop();
        }

        protected abstract IPrimeFactory CreateFactory();
    }
}
