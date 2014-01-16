using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace OptimusPrimeTest.Factory
{
    [Serializable]
    public class TestSerializationData : ISmartCloneTestData
    {
        public TestSerializationData()
        {
            ValueMember = Guid.NewGuid();
            LinkedMember = TestData.CreateData(1)[0];
        }

        public TestSerializationData(Guid valueMember, TestData linkedMember)
        {
            ValueMember = valueMember;
            LinkedMember = linkedMember;
        }

        public Guid ValueMember { get; set; }
        public TestData LinkedMember { get; set; }

        public ISmartCloneTestData CreateCopy()
        {
            return new TestSerializationData(ValueMember, LinkedMember.Clone());
        }

        public void AssertAreEquals(ISmartCloneTestData actual)
        {
            Assert.AreEqual(ValueMember, actual.ValueMember);
            LinkedMember.AssertAreEqual(actual.LinkedMember);
        }

        public static List<TestSerializationData> CreateDataList(int count)
        {
            var result = new List<TestSerializationData>();
            for (int i = 0; i < count; i++)
                result.Add(new TestSerializationData());
            return result;
        }
    }
}