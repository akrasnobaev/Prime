using System;
using System.Collections.Generic;
using NUnit.Framework;
using Prime;

namespace OptimusPrimeTest.LibertyPrime
{
    [TestFixture]
    public class SmartCloneFactoryTest
    {
        [Test]
        public void TestSmartCloneInSourceReader()
        {
            var testDatas = TestClonableData.CreateDataList(4);
            var factory = new LibertyFactory();
            var sourceBlock = new EventBlock<TestClonableData>();
            var sourse = factory.CreateSource(sourceBlock);

            factory.Start();
            // publish data
            foreach (var testData in testDatas)
                sourceBlock.Publish(testData);

            // copy and change data
            var copyTestDatas = new List<TestClonableData>();
            foreach (var data in testDatas)
            {
                copyTestDatas.Add((TestClonableData) data.CreateCopy());
                data.ValueMember = Guid.NewGuid();
                data.LinkedMember.Number++;
            }

            // test TryGet()
            var reader = sourse.Factory.CreateReciever(sourse).GetReader();
            TestClonableData actual;
            reader.TryGet(out actual);
            copyTestDatas[0].AssertAreEquals(actual);

            // test Get()
            actual = reader.Get();
            copyTestDatas[1].AssertAreEquals(actual);

            // test GetCollection()
            var actualCollections = reader.GetCollection();
            Assert.AreEqual(copyTestDatas.Count - 2, actualCollections.Length);
            for (int i = 2; i < copyTestDatas.Count; i++)
                copyTestDatas[i].AssertAreEquals(actualCollections[i - 2]);

            factory.Stop();
        }

        [Test]
        public void testSmartCloneInLibertyChain()
        {
            var testData = new TestClonableData();
            var factory = new LibertyFactory();
            var chain = factory.CreateChain<TestClonableData, TestClonableData>(data => data);
            var function = chain.ToFunctionalBlock();


            factory.Start();

            var copyTestData = testData.CreateCopy();
            var actual = function.Process(testData);

            testData.ValueMember = Guid.NewGuid();
            testData.LinkedMember.Number ++;

            copyTestData.AssertAreEquals(actual);

            factory.Stop();
        }

        [Test, ExpectedException(typeof (DataCanNotBeClonnedPrimeException))]
        public void TestSmartCloneFailInSourceReader()
        {
            var factory = new LibertyFactory();
            var sourceBlock = new EventBlock<NonClonableTestData>();
            var source = factory.CreateSource(sourceBlock);
            source.Factory.CreateReciever(source).GetReader();
        }

        [Test, ExpectedException(typeof (DataCanNotBeClonnedPrimeException))]
        public void TestSmartCloneFailInLibertyChain()
        {
            var factory = new LibertyFactory();
            factory.CreateChain<int, NonClonableTestData>(a => new NonClonableTestData());
        }
    }
}