using System;
using NUnit.Framework;
using OptimusPrime.Factory;
using OptimusPrime.Templates;

namespace OptimusPrimeTests.Generics
{
    [TestFixture]
    public abstract class ForkRepeaterTestBase
    {
        private class CharSourceCollection : SyncronousSourceDataCollection<char>
        {
        }

        private class RepeaterForFork : IRepeaterBlock<string, string, char, char, CharSourceCollection>
        {
            private string input;
            private int position;


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

        [Test]
        public void TestForkRepeater()
        {
            var factory = CreateFactory();
            var privateChain = factory.CreateChain(new Func<char, char>(c => c));
            var fork = privateChain.Fork();
            var collector = factory.BindSources(fork.Source).CreateSyncCollector<CharSourceCollection>();
            var repeater = factory.CreateRepeater(new RepeaterForFork(), collector, fork.Chain);
            factory.Start();
            repeater.ToFunctionalBlock().Process("abcdefghij");
            factory.Stop();
        }

        protected abstract IFactory CreateFactory();
    }
}
