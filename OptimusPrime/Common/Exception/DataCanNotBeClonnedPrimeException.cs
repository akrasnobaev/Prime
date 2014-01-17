using System;

namespace Prime
{
    public class DataCanNotBeClonnedPrimeException : PrimeException
    {
        public DataCanNotBeClonnedPrimeException(Type type)
            : base(string.Format("Data of type {0} can not be cloned", type.Name))
        {
        }
    }
}