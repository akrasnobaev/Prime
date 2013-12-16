using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptimusPrime.Generics
{
    public class Token
    {
        private Token() { }
        public static readonly Token Empty = new Token();
    }
}
