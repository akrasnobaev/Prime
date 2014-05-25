using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OptimusPrimeTest.Prime
{
    [Serializable]
    // Important: This attribute is NOT inherited from Exception, and MUST be specified 
    // otherwise serialization will fail with a SerializationException stating that
    // "Type X in Assembly Y is not marked as serializable."
    public class TestException : Exception
    {
        private readonly TestData testData;

        public TestException()
        {
        }

        public TestException(string message)
            : base(message)
        {
        }

        public TestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public TestException(string message, TestData testData)
            : base(message)
        {
            this.testData = testData;
        }

        public TestException(string message, TestData testData, Exception innerException)
            : base(message, innerException)
        {
            this.testData = testData;
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        // Constructor should be protected for unsealed classes, private for sealed classes.
        // (The Serializer invokes this constructor through reflection, so it can be private)
        protected TestException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            testData = (TestData)info.GetValue("testData", typeof(TestData));
        }


        public TestData TestData
        {
            get { return testData; }
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("testData", TestData, typeof(TestData));

            // MUST call through to the base class to let it save its own state
            base.GetObjectData(info, context);
        }
    }
}