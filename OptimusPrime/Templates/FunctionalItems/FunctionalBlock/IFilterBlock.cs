using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptimusPrime.Templates
{
    interface IFilterBlock<T>
    {
        bool Pass(T data);
    }
}
