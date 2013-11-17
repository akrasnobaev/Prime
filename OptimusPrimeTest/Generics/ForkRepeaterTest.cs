using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OptimusPrime.Factory;
using OptimusPrime.Templates;

namespace OptimusPrimeTests.Generics
{
    [TestFixture]
    class ForkRepeaterTest
    {
        class CharSourceCollection : SourceDataCollection<char>
        {
            public List<char> Chars { get { return base.List0; } }
        }

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
                Assert.NotNull(sourceDatas.Chars);
                Assert.AreEqual(1, sourceDatas.Chars.Count);
                Assert.AreEqual(oldPrivateOut, sourceDatas.Chars[0]);
                if (position >= input.Length)
                {
                    privateIn = '\0';
                    return false;
                }
                privateIn = input[position];
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
            var collector = factory.BindSources(fork.Source).CreateCollector<CharSourceCollection>();
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
