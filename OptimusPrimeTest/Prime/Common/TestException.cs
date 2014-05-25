using System;
using System.Runtime.Serialization;

namespace OptimusPrimeTest.Prime
{
    [Serializable]
    public class TestException : Exception
    {
        public TestException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public TestException(String message) : base(message)
        {
        }
    }
}