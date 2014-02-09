using System;
using NUnit.Framework;
using OptimusPrimeTest.LibertyPrime;
using Prime;

namespace OptimusPrimeTests.LibertyPrime.Factory.SmartClone
{
    [TestFixture]
    public class SmartCloneTest
    {
        [Test]
        public void TestCloneString()
        {
            var smartClone = new SmartClone<string>();
            var data = "AnyString";
            var actual = smartClone.Clone(data);
            data = "OtherString";

            Assert.AreEqual("AnyString", actual);
        }

        [Test]
        public void TestCloneInteger()
        {
            var smartClone = new SmartClone<int>();
            var data = 11;
            var actual = smartClone.Clone(data);
            data = 22;

            Assert.AreEqual(11, actual);
        }

        [Test]
        public void TestCloneImmutableData()
        {
            var smartClone = new SmartClone<TestImmutableData>();
            var data = new TestImmutableData();
            var expected = data.CreateCopy();
            var actual = smartClone.Clone(data);
            data.ValueMember = new Guid();
            data.LinkedMember.Name = "OtherName";

            expected.AssertAreEquals(actual);
        }

        [Test]
        public void TestCloneClonableData()
        {
            var smartClone = new SmartClone<TestClonableData>();
            var data = new TestClonableData();
            var expected = data.CreateCopy();
            var actual = smartClone.Clone(data);
            data.ValueMember = new Guid();
            data.LinkedMember.Name = "OtherName";

            expected.AssertAreEquals(actual);
        }

        [Test]
        public void TestCloneSerializableData()
        {
            var smartClone = new SmartClone<TestSerializationData>();
            var data = new TestSerializationData();
            var expected = data.CreateCopy();
            var actual = smartClone.Clone(data);
            data.ValueMember = new Guid();
            data.LinkedMember.Name = "OtherName";

            expected.AssertAreEquals(actual);
        }
    }
}