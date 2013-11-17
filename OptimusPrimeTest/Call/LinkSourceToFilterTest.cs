﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Eurobot.Services;
using NUnit.Framework;
using OptimusPrime.Factory;
using OptimusPrime.Templates;

namespace OptimusPrimeTest.Call
{
    [TestFixture]
    public class LinkSourceToFilterTest
    {
        private CallFactory factory;
        private SourceBlock<TestData> sourceBlock;
        private AutoResetEvent isReadFinished;
        private List<TestData> sourseData;
        private List<TestData> resultData;
        private ISourceReader<TestData> sourceReader;


        [SetUp]
        public void SetUp()
        {
            factory = new CallFactory();
            isReadFinished = new AutoResetEvent(false);
        }

        [Test]
        public void TestGet()
        {
            var chain = new FunctionalBlock<TestData, bool>(IsEven);
            sourceBlock = new SourceBlock<TestData>();
            var source = factory.CreateSource(sourceBlock);

            var testSource = factory.LinkSourceToFilter(source, chain);
            sourseData = TestData.CreateData(100);
            sourceReader = testSource.CreateReader();
            resultData = sourseData.Where(z => z.Number % 2 == 0).ToList();

            factory.Start();

            new Thread(() => WriteData(true)).Start();
            new Thread(() => ReadData(true)).Start();
            isReadFinished.WaitOne();

            TestData outTestData;
            Assert.IsFalse(sourceReader.TryGet(out outTestData));
        }

        private void ReadData(bool isWait)
        {
            var random = new Random();
            foreach (var testData in resultData)
            {
                if(isWait)
                    Thread.Sleep(random.Next(10));
                var actual = sourceReader.Get();
                testData.AssertAreEqual(actual);
            }
            isReadFinished.Set();
        }

        private void WriteData(bool isWait = false)
        {
            var random = new Random();
            foreach (var data in sourseData)
            {
                if(isWait)
                    Thread.Sleep(random.Next(10));
                sourceBlock.Publish(data);
            }
        }

        private static bool IsEven(TestData input)
        {
            var result = input.Clone();
            return result.Number % 2 == 0;
        }

        [TearDown]
        public void TearDown()
        {
            factory.Stop();
        }
    }
}