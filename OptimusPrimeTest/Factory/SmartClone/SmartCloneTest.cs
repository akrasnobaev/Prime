using System;
using System.Collections.Generic;
using NUnit.Framework;
using Prime;

namespace OptimusPrimeTest.Factory
{
    [TestFixture]
    public class SmartCloneTest
    {
        private void testSmartCloneInSourceReader<T>(List<T> testDatas)
            where T : ISmartCloneTestData
        {
            var factory = new LibertyFactory();
            var sourceBlock = new SourceBlock<T>();
            var sourse = factory.CreateSource(sourceBlock);

            factory.Start();
            // publish data
            foreach (var testClonableData in testDatas)
                sourceBlock.Publish(testClonableData);

            // copy and change data
            var copyTestDatas = new List<T>();
            foreach (var data in testDatas)
            {
                copyTestDatas.Add((T) data.CreateCopy());
                data.ValueMember = Guid.NewGuid();
                data.LinkedMember.Number++;
            }

            // test TryGet()
            var reader = sourse.CreateReader();
            T actual;
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
        public void TestSmartCloneClonableDataInSourceReader()
        {
            var testClonableDatas = TestClonableData.CreateDataList(4);
            testSmartCloneInSourceReader(testClonableDatas);
        }

        [Test]
        public void TestSmartCloneSerializableDataInSourseReader()
        {
            var testSerializationDatas = TestSerializationData.CreateDataList(4);
            testSmartCloneInSourceReader(testSerializationDatas);
        }

        private void testSmartCloneInLibertyChain<T>(T testData)
            where T : ISmartCloneTestData
        {
            var factory = new LibertyFactory();
            var chain = factory.CreateChain<T, T>(data => data);
            var function = chain.ToFunctionalBlock();


            factory.Start();

            var copyTestData = testData.CreateCopy();
            var actual = function.Process(testData);

            testData.ValueMember = Guid.NewGuid();
            testData.LinkedMember.Number ++;

            copyTestData.AssertAreEquals(actual);

            factory.Stop();
        }

        [Test]
        public void TestSmartCloneClonableDataInLibertyChain()
        {
            var testData = new TestClonableData();
            testSmartCloneInLibertyChain(testData);
        }

        [Test]
        public void TestSmartCloneSerializableDataInLibertyChain()
        {
            var testData = new TestSerializationData();
            testSmartCloneInLibertyChain(testData);
        }

        [Test, ExpectedException(typeof (DataCanNotBeClonnedPrimeException))]
        public void TestSmartCloneFailInSourceReader()
        {
            var factory = new LibertyFactory();
            var sourceBlock = new SourceBlock<NonClonableTestData>();
            var source = factory.CreateSource(sourceBlock);
            source.CreateReader();
        }

        [Test, ExpectedException(typeof (DataCanNotBeClonnedPrimeException))]
        public void TestSmartCloneFailInLibertyChain()
        {
            var factory = new LibertyFactory();
            factory.CreateChain<NonClonableTestData, int>(data => 1);
        }
    }
}