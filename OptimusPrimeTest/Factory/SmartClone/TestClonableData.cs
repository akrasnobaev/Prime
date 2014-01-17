using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace OptimusPrimeTest.Factory
{
    public class TestClonableData : ICloneable, ISmartCloneTestData
    {
        public TestClonableData()
        {
            ValueMember = Guid.NewGuid();
            LinkedMember = TestData.CreateData(1)[0];
        }

        private TestClonableData(Guid valueMember, TestData linkedMember)
        {
            ValueMember = valueMember;
            LinkedMember = linkedMember;
        }

        public object Clone()
        {
            return CreateCopy();
        }

        public Guid ValueMember { get; set; }
        public TestData LinkedMember { get; set; }

        public ISmartCloneTestData CreateCopy()
        {
            return new TestClonableData(ValueMember, LinkedMember.Clone());
        }

        public void AssertAreEquals(ISmartCloneTestData actual)
        {
            Assert.AreEqual(ValueMember, actual.ValueMember);
            LinkedMember.AssertAreEqual(actual.LinkedMember);
        }

        public static List<TestClonableData> CreateDataList(int count)
        {
            var result = new List<TestClonableData>();
            for (int i = 0; i < count; i++)
                result.Add(new TestClonableData());
            return result;
        }
    }
}