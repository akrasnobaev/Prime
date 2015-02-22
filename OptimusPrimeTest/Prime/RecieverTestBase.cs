using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Prime;


namespace OptimusPrimeTest.Prime
{
    [TestFixture]
    abstract class RecieverTestBase
    {
        private List<int> _testDatas = new List<int>{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
        private const int waitTime = 5;

        protected abstract IPrimeFactory CreateFactory();

        protected abstract IReciever<int> CreateMockReciever(string input);

        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void TestGet()
        {
            var factory = CreateFactory();
            var reciever = PublishSomeData(factory).CreateReciever("readLog");

            var expected = new List<int>();
            var reader = reciever.GetReader();
            for (int i =  0; i < _testDatas.Count; ++i)
            {
                expected.Add(reader.Get());
            }

            factory.Stop();

            var mock = CreateMockReciever("readLog");
            var actual = new List<int>();
            var mockReader = mock.GetReader();
            for (int i = 0; i < _testDatas.Count; ++i)
            {
                try
                {
                    actual.Add(mockReader.Get());
                }
                catch { break;}
            }

            for (int i = 0; i < _testDatas.Count; ++i)
            {
                Assert.True(_testDatas[i] == actual[i]);
            }
        }

        [Test]
        public void TestGetCollection()
        {
            var factory = CreateFactory();
            var sourceBlock = new EventBlock<int>();
            var source = factory.CreateSource(sourceBlock, "someSource2");

            factory.Start();

            for (int i = 0; i < _testDatas.Count / 2; ++i)
            {
                Thread.Sleep(waitTime);
                sourceBlock.Publish(_testDatas[i]);
            }

            var reciever = source.CreateReciever("readLog2");
            var expected1 = reciever.GetReader().GetCollection();

            for (int i = _testDatas.Count; i < _testDatas.Count; ++i)
            {
                Thread.Sleep(waitTime);
                sourceBlock.Publish(_testDatas[i]);
            }

            var expected2 = reciever.GetReader().GetCollection();

            factory.Stop();

            var mock = CreateMockReciever("readLog2");
            Assert.AreSame(expected1, mock.GetReader().GetCollection());
            Assert.AreSame(expected2, mock.GetReader().GetCollection());
        }

        [Test]
        public void TestTryGet()
        {
            var factory = CreateFactory();
            var reciever = PublishSomeData(factory).CreateReciever("readLog3");

            var expected = new List<int>();
            var reader = reciever.GetReader();
            for (int i = 0; i < _testDatas.Count; ++i)
            {
                expected.Add(reader.Get());
            }

            factory.Stop();

            var mock = CreateMockReciever("readLog3");
            var actual = new List<int>();
            var mockReader = mock.GetReader();
            for (int i = 0; i < _testDatas.Count; ++i)
            {
                try
                {
                    int data;
                    Assert.IsTrue(mockReader.TryGet(out data));
                    actual.Add(data);
                }
                catch { break; }
            }


            for (int i = 0; i < _testDatas.Count; ++i)
            {
                Assert.True(_testDatas[i] == actual[i]);
            }

        }

        private ISource<int> PublishSomeData(IPrimeFactory factory)
        {
            var sourceBlock = new EventBlock<int>();
            var source = factory.CreateSource(sourceBlock, "someSource" + n++);

            factory.Start();

            foreach (int data in _testDatas)
            {
                Thread.Sleep(waitTime);
                sourceBlock.Publish(data);
            }

            /// Ожидание окончания работы всех цепочек.
            /// Делать общий механизм опоыещения об оконцании работы цепочек пока нет смысла.
            Thread.Sleep(100);

            return source;
        }

        private int n = 0;
    }
}
