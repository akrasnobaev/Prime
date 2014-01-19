using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace OptimusPrimeTest.Prime
{
    [Serializable]
    public class TestData
    {
        public TestData()
        {
            Id = Guid.NewGuid();
        }

        public TestData(Guid id, string name, int number)
        {
            Id = id;
            Name = name;
            Number = number;
        }

        public Guid Id { get; private set; }
        public string Name { get; set; }
        public int Number { get; set; }

        public void AssertAreEqual(TestData second)
        {
            Assert.AreEqual(Id, second.Id);
            Assert.AreEqual(Name, second.Name);
            Assert.AreEqual(Number, second.Number);
        }

        public TestData Clone()
        {
            return new TestData(Id, Name, Number);
        }

        public static List<TestData> CreateData(int count)
        {
            var datas = new List<TestData>();
            for (int i = 0; i < count; i++)
                datas.Add(new TestData { Name = i.ToString(), Number = i });
            return datas;
        }
    }
}