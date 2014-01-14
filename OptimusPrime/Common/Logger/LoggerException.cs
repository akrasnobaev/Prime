using System;

namespace Prime
{
    public class LoggerException : Exception
    {
        public LoggerException(string message) : base(message)
        {
        }
    }
}