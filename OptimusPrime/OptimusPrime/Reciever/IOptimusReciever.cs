using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Prime.Optimus
{
    public interface IOptimusReciever<T> : IReciever<T>
    {
        IOptimusIn Input { get; }
    }
}
