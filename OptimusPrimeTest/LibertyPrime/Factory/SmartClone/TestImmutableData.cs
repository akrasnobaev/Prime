using System;
using System.Collections.Generic;
using System.ComponentModel;
using NUnit.Framework;
using OptimusPrimeTest.Prime;

namespace OptimusPrimeTest.LibertyPrime
{
    [ImmutableObject(true)]
    public class TestImmutableData : ISmartCloneTestData
    {
        private readonly Guid valueMember;
        private readonly TestData linkedMember;

        public TestImmutableData()
        {
            valueMember = Guid.NewGuid();
            linkedMember = TestData.CreateData(1)[0];
        }

        private TestImmutableData(Guid valueMember, TestData linkedMember)
        {
            this.valueMember = valueMember;
            this.linkedMember = linkedMember;
        }

        public Guid ValueMember {
            get { return valueMember; }
            set { }
        }
        public TestData LinkedMember {
            get { return linkedMember.Clone();}
            set { }
        }

        public ISmartCloneTestData CreateCopy()
        {
            return new TestImmutableData(valueMember, linkedMember.Clone());
        }

        public void AssertAreEquals(ISmartCloneTestData actual)
        {
            Assert.AreEqual(valueMember, actual.ValueMember);
            linkedMember.AssertAreEqual(actual.LinkedMember);
        }

        public static List<TestImmutableData> CreateDataList(int count)
        {
            var result = new List<TestImmutableData>();
            for (int i = 0; i < count; i++)
                result.Add(new TestImmutableData());
            return result;
        }
    }
}